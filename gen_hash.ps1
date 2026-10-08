$ErrorActionPreference = "Stop"

function Generate-Hash($password) {
    $salt = New-Object byte[] 16
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $rng.GetBytes($salt)
    $derive = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($password, $salt, 210000)
    $hash = $derive.GetBytes(32)
    $prefix = 'PBKDF2-SHA1' + [char]36 + '210000' + [char]36
    $result = $prefix + [Convert]::ToBase64String($salt) + [char]36 + [Convert]::ToBase64String($hash)
    $derive.Dispose()
    $rng.Dispose()
    return $result
}

Write-Output "admin: $(Generate-Hash 'Admin@123')"
Write-Output "dispatcher: $(Generate-Hash 'Dispatcher@123')"
Write-Output "cashier: $(Generate-Hash 'Cashier@123')"
