using System;

namespace Noc_App.Helpers
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
                        <p>Best Regards,<br><br>
                        Department of Water Resources, Punjab</p>
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
                        <p>Best Regards,<br><br>
                        Department of Water Resources, Punjab</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }

        public static string EmailStringBodyForGrantMessage(string applicantName,string applicationID)
        {
            return $@"<htm>
            <head></head>
            <body style=""margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;"">
            <div style=""height:auto;background:linear-gradient: to top #c9c9ff 50% #6e6ef6 90%) no-repeat;width:400px;padding:30px;"">
                <div>
                    <div>
                        <p>Dear {applicantName},</p>
                        
                        <p>Congratulations! 🎉 Your application for the NOC Certificate with ID <b>{applicationID}</b> has been successfully submitted.</p><p>Keep track of its progress easily by using this Application ID on our portal.</p><br>
                        <p>Best Regards,<br><br>
                        Department of Water Resources, Punjab</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }

        public static string EmailStringBodyForRejection(string applicantName, string applicationID, string reason)
        {
            return $@"<htm>
            <head></head>
            <body style=""margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;"">
            <div style=""height:auto;background:linear-gradient: to top #c9c9ff 50% #6e6ef6 90%) no-repeat;width:400px;padding:30px;"">
                <div>
                    <div>
                        <p>Dear {applicantName},</p>
                        
                        <p>Your application for the NOC Certificate with ID <b>{applicationID}</b> has been rejected due to {reason}.</p><p>Please apply again to get NOC certificate</p><br>
                        <p>Best Regards,<br><br>
                        Department of Water Resources, Punjab</p>
                    </div>
                </div>
            </div>
            </body>
            </html>";
        }
    }
}
