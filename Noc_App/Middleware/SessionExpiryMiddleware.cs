﻿using Noc_App.Helpers;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.UtilityService;

namespace Noc_App.Middleware
{
    public class SessionExpiryMiddleware
    {

        //private readonly IRepository<UserSessionDetails> _userSessionnRepository;
        private readonly RequestDelegate _next;
        //private readonly ITokenService _tokenService;

        public SessionExpiryMiddleware(RequestDelegate next/*, ITokenService tokenService, IRepository<UserSessionDetails> userSessionnRepository*/)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            //_tokenService = tokenService;
            //_userSessionnRepository = userSessionnRepository ?? throw new ArgumentNullException(nameof(userSessionnRepository));
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Allow unrestricted access to login and register pages
            if (context.Request.Path.StartsWithSegments("/") || context.Request.Path.StartsWithSegments("/Account") || context.Request.Path.StartsWithSegments("/Payment")
                || context.Request.Path.StartsWithSegments("/Grant"))
            {
                await _next(context);
                return;
            }
            // Get the user session ID
            var userId = context.Session.GetString("Userid");

            // Check if user is logged in or session is invalid
            if (string.IsNullOrEmpty(userId))
            {
                if (context.Request.Path.StartsWithSegments("/Grant/Create") || context.Request.Path.StartsWithSegments("/Grant/TrackStatus") || context.Request.Path.StartsWithSegments("/Grant/UserDownload") || context.Request.Path.StartsWithSegments("/Grant/Modify"))
                {
                    await _next(context);
                    return;
                }
                else
                {
                    // Clear session and redirect to login if session is expired
                    context.Session.Clear();
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }
            
            var session = context.Session;
            if (session != null && session.IsAvailable)
            {
                // The session is still active, you can perform any actions here
                // For example, reset session timeout on each request to prevent expiry
            

           // var token = context.Session.GetString("SessionToken");

            //// Validate the token
            //if (!string.IsNullOrEmpty(token) && _tokenService.ValidateToken(token))
            //{
            //    // Token is valid, rotate the token
            //    _tokenService.RotateToken(context);
            //}
            //else
            //{
            //    context.Session.Clear();
            //    context.Response.Redirect("/Account/Login");
            //    return;
            //}
                //// Get session information from HttpContext session
                //var storedSessionId = context.Session.GetString("SessionId");
                //var hrms = context.Session.GetString("Userid");

                //// Generate a new session ID for the user based on HttpContext
                ////var ss = _userSessionnRepository;
                ////List<UserSessionDetails> sessionDetail = _userSessionnRepository.GetAll()
                ////                                         .Where(u => u.Hrms == hrms)
                ////                                         .ToList();

                ////string storedSessionId = sessionDetail.Count > 0 ? sessionDetail[0].LastSessionId : "";
                //var currentSessionId = SessionHelper.GenerateSessionId(context);

                //// If session ID doesn't match, invalidate the session and redirect to login
                //if (storedSessionId != null)
                //{
                //    var storedSessionTime = context.Session.GetString("SessionTime");
                //    DateTime datetime = Convert.ToDateTime(storedSessionTime);
                //    TimeSpan difference = DateTime.Now - datetime;
                //    var hour = difference.TotalHours;
                //    if(hour>=1)
                //    if (currentSessionId != storedSessionId)
                //    {
                //        context.Session.Clear();
                //        context.Response.Redirect("/Account/Login");
                //        return;
                //    }
                //}

                //// Store the session ID in the session (can be used for validation on future requests)
                //context.Session.SetString("SessionId", currentSessionId);
            }
            else
            {
                // Session has expired, clear it and redirect to login page
                context.Session.Clear(); // Explicitly clear session if expired
                context.Response.Redirect("/Account/Login"); // Redirect user to login
                return;
            }
            await _next(context);
        }
    }
}
