param(
    [string]$Login = "admin"
)

$ErrorActionPreference = "Stop"

function Get-CryptoIndex {
    param(
        [int]$Maximum,
        [System.Security.Cryptography.RandomNumberGenerator]$Random
    )

    $bytes = New-Object byte[] 4
    $range = [uint64]4294967296
    $limit = $range - ($range % [uint64]$Maximum)

    do {
        $Random.GetBytes($bytes)
        $value = [uint64][System.BitConverter]::ToUInt32($bytes, 0)
    } while ($value -ge $limit)

    return [int]($value % [uint64]$Maximum)
}

function Get-RandomCharacter {
    param(
        [string]$Characters,
        [System.Security.Cryptography.RandomNumberGenerator]$Random
    )

    return $Characters[(Get-CryptoIndex -Maximum $Characters.Length -Random $Random)]
}

$confirmation = Read-Host "This will replace the password for '$Login'. Type RESET to continue"
if ($confirmation -cne "RESET") {
    throw "Password bootstrap cancelled."
}

$random = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$deriveBytes = $null
$connection = $null
$transaction = $null

try {
    $upper = "ABCDEFGHJKLMNPQRSTUVWXYZ"
    $lower = "abcdefghijkmnopqrstuvwxyz"
    $digits = "23456789"
    $symbols = "!#$%&*+-=?@_"
    $alphabet = $upper + $lower + $digits + $symbols
    $characters = New-Object 'System.Collections.Generic.List[char]'

    $characters.Add([char](Get-RandomCharacter -Characters $upper -Random $random))
    $characters.Add([char](Get-RandomCharacter -Characters $lower -Random $random))
    $characters.Add([char](Get-RandomCharacter -Characters $digits -Random $random))
    $characters.Add([char](Get-RandomCharacter -Characters $symbols -Random $random))
    while ($characters.Count -lt 24) {
        $characters.Add([char](Get-RandomCharacter -Characters $alphabet -Random $random))
    }

    for ($index = $characters.Count - 1; $index -gt 0; $index--) {
        $swapIndex = Get-CryptoIndex -Maximum ($index + 1) -Random $random
        $temporary = $characters[$index]
        $characters[$index] = $characters[$swapIndex]
        $characters[$swapIndex] = $temporary
    }

    $password = -join $characters
    $salt = New-Object byte[] 16
    $random.GetBytes($salt)
    $iterations = 210000
    $deriveBytes = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($password, $salt, $iterations)
    $hash = $deriveBytes.GetBytes(32)
    $passwordHash = "PBKDF2-SHA1`$$iterations`$$([Convert]::ToBase64String($salt))`$$([Convert]::ToBase64String($hash))"

    $configPath = Join-Path $PSScriptRoot "..\Web.config"
    [xml]$webConfig = Get-Content -LiteralPath $configPath
    $connectionNode = $webConfig.SelectSingleNode("/configuration/connectionStrings/add[@name='AutoparkDBConnection']")
    if ($null -eq $connectionNode) {
        throw "Connection string 'AutoparkDBConnection' was not found in Web.config."
    }

    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionNode.GetAttribute("connectionString"))
    $connection.Open()
    $transaction = $connection.BeginTransaction()

    $command = $connection.CreateCommand()
    $command.Transaction = $transaction
    $command.CommandText = @"
UPDATE account
SET password_hash = @password_hash
FROM dbo.app_user AS account
WHERE account.login = @login
  AND account.is_active = 1
  AND EXISTS
  (
      SELECT 1
      FROM dbo.user_role AS userRole
      INNER JOIN dbo.app_role AS appRole ON appRole.role_id = userRole.role_id
      WHERE userRole.user_id = account.user_id
        AND appRole.role_name = 'administrator'
        AND appRole.is_active = 1
  );
"@

    $loginParameter = $command.Parameters.Add("@login", [System.Data.SqlDbType]::VarChar, 80)
    $loginParameter.Value = $Login
    $hashParameter = $command.Parameters.Add("@password_hash", [System.Data.SqlDbType]::VarChar, 256)
    $hashParameter.Value = $passwordHash

    $rowsUpdated = $command.ExecuteNonQuery()
    $command.Dispose()

    if ($rowsUpdated -ne 1) {
        $transaction.Rollback()
        $transaction = $null
        throw "No active administrator account named '$Login' was updated. Check the login and administrator role assignment."
    }

    $transaction.Commit()
    $transaction = $null

    Write-Host "Password updated for administrator '$Login'." -ForegroundColor Green
    Write-Host "Initial password (shown once): $password" -ForegroundColor Yellow
    Write-Host "Store this password securely. Run this script again to rotate it."
}
finally {
    if ($null -ne $transaction) {
        $transaction.Dispose()
    }
    if ($null -ne $connection) {
        $connection.Dispose()
    }
    if ($null -ne $deriveBytes) {
        $deriveBytes.Dispose()
    }
    $random.Dispose()
}