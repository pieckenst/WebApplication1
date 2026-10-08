        public static string ClientIpAddress
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return ip;
            }
        }

        #endregion

        #region Authentication Methods

        public static bool RefreshAuthorization()
        {
            if (!IsAuthenticated || HttpContext.Current == null || HttpContext.Current.Session == null)
                return false;

            int? userId = CurrentUserId;
            if (!userId.HasValue)
                return false;

            User user;
            List<string> roles;
            List<string> permissions;
            using (UserRepository repository = new UserRepository())
            {
                user = repository.GetUserById(userId.Value);
                if (user == null || !user.IsActive)
                    return false;

                roles = repository.GetUserRoles(user.UserId);
                permissions = repository.GetUserPermissions(user.UserId);
            }

            SetSessionValue(SESSION_KEY_USER_ID, user.UserId);
            SetSessionValue(SESSION_KEY_USERNAME, user.Login);
            SetSessionValue(SESSION_KEY_EMPLOYEE_ID, user.EmployeeId);
            SetSessionValue(SESSION_KEY_ROLES, roles);
            SetSessionValue(SESSION_KEY_PERMISSIONS, permissions);

            // SignIn intentionally rotates the ASP.NET session ID to avoid session fixation.
            // Session.Abandon() means the new session may not exist until the next request,
            // so restore the login/activity timestamps from the Forms Authentication ticket
            // before SecurePage performs the inactivity check.
            FormsIdentity formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            DateTime authenticationTime = formsIdentity != null
                ? formsIdentity.Ticket.IssueDate.ToLocalTime()
                : DateTime.Now;

            if (!LoginTime.HasValue)
                SetSessionValue(SESSION_KEY_LOGIN_TIME, authenticationTime);

            if (!LastActivityTime.HasValue)
                SetSessionValue(SESSION_KEY_LAST_ACTIVITY, authenticationTime);

            IPrincipal currentPrincipal = HttpContext.Current.User;
            GenericPrincipal principal = new GenericPrincipal(currentPrincipal.Identity, roles.ToArray());
            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
            return true;
        }

        /// <summary>
        /// Authenticates a user and creates a session.
        /// </summary>
        /// <param name="user">The user object to authenticate</param>
        /// <param name="roles">User's roles</param>
        /// <param name="permissions">User's permissions</param>
        /// <param name="rememberMe">Whether to create a persistent cookie</param>
        /// <param name="employeeId">Optional linked employee ID</param>
        public static void SignIn(User user, List<string> roles, List<string> permissions, 
            bool rememberMe = false, int? employeeId = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (HttpContext.Current == null)
                throw new InvalidOperationException("HttpContext is not available");

            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                SessionIDManager sessionIdManager = new SessionIDManager();
                string sessionId = sessionIdManager.CreateSessionID(HttpContext.Current);