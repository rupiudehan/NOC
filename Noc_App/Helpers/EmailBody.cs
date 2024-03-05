﻿namespace Noc_App.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBodyForReset(string email, string emailToken)
        {
            return $@"<htm>
            <head></head>
            <body style=""margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;"">
            <div style=""height:auto;background:linear-gradient: to top #c9c9ff 50% #6e6ef6 90%) no-repeat;width:400px;padding:30px;"">
                <div>
                    <div>
                        <h1>Reset your password</h1>
                        <hr>
                        <p>You getting this email because you requested a password reset for your account.</p>
                        <p>Please tap the button below to choose a new password.</p>
                        <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none;
                        color:white;border-radius:4px;display:block;margin:0 auto; width:50%;text-align:center;text-decoration:none;"">Reset Password</a><br>
                        <p>Kind Regards,<br><br>
                        Official</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }
        public static string EmailStringBodyForInformation(string email, string password)
        {
            return $@"<htm>
            <head></head>
            <body style=""margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;"">
            <div style=""height:auto;background:linear-gradient: to top #c9c9ff 50% #6e6ef6 90%) no-repeat;width:400px;padding:30px;"">
                <div>
                    <div>
                        <h1>Login Credentials</h1>
                        <hr>
                        <p>Username: {email}</p>
                        <p>Password: {password}</p>
                        <br>
                        <p>Kind Regards,<br><br>
                        Official</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }

        public static string EmailStringBodyForGrantMessage(string email,string applicationID)
        {
            return $@"<htm>
            <head></head>
            <body style=""margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;"">
            <div style=""height:auto;background:linear-gradient: to top #c9c9ff 50% #6e6ef6 90%) no-repeat;width:400px;padding:30px;"">
                <div>
                    <div>
                        <h1>Grant Application Status</h1>
                        <hr>
                        <p>You getting this email because you applied for NOC Grant.</p>
                        <p>Application for Grant with Application ID <b>{applicationID}</b> has been applied successfully!</p><br>
                        <p>Kind Regards,<br><br>
                        Official</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }
    }
}
