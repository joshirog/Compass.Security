using System;
using System.Linq;
using System.Threading.Tasks;
using Compass.Security.Application.Services.Accounts.Commands.Confirm;
using Compass.Security.Application.Services.Accounts.Commands.External;
using Compass.Security.Application.Services.Accounts.Commands.Otp;
using Compass.Security.Application.Services.Accounts.Commands.Recovery;
using Compass.Security.Application.Services.Accounts.Commands.Resend;
using Compass.Security.Application.Services.Accounts.Commands.Reset;
using Compass.Security.Application.Services.Accounts.Commands.SignIn;
using Compass.Security.Application.Services.Accounts.Commands.SignOut;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using Compass.Security.Application.Services.Accounts.Commands.TwoFactor;
using Compass.Security.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class AccountController : BaseWebController
    {
        public IActionResult SignIn(string returnUrl)
        {
            if (User.Identity is not { IsAuthenticated: true })
            {
                return View(new SignInCommand { ReturnUrl = returnUrl });
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                TempData["message"] = response.Message;

                return response.Success switch
                {
                    true when response.Data.IsOtp => RedirectToAction(nameof(TwoStep),
                        new { response.Data.Id, command.ReturnUrl }),
                    true when !string.IsNullOrEmpty(command.ReturnUrl) => Redirect(command.ReturnUrl),
                    true => RedirectToAction("Index", "Home"),
                    _ => View(command)
                };
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SignOut(string returnUrl)
        {
            await Mediator.Send(new SignOutCommand());
            
            if(!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("SignIn", "Account");
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var response = await Mediator.Send(new ExternalCommand {Provider = provider, ReturnUrl = returnUrl});

            var data = new ChallengeResult(provider, response.Data);
            
            return data;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var response = await Mediator.Send(new CallbackCommand { ReturnUrl = returnUrl, RemoteError = remoteError });

            if (!response.Success)
            {
                TempData["message"] = response.Message;
                
                return View("SignIn", new SignInCommand { ReturnUrl = returnUrl, });
            }

            if(string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");
                
            return LocalRedirect(returnUrl);
        }
        
        public IActionResult SignUp(string returnUrl)
        {
            return View(new SignUpCommand { ReturnUrl = returnUrl });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
            
                if(response.Success)
                    TempData["message"] = response.Message;
            
                return response.Success switch
                {
                    true when !string.IsNullOrEmpty(command.ReturnUrl) => Redirect(command.ReturnUrl),
                    true => RedirectToAction("Resend", new
                    {
                        id = response.Data.UserId,
                        returnUrl = command.ReturnUrl
                    }),
                    _ => View(command)
                };
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }
        
        public IActionResult Resend(string id, string returnUrl)
        {
            return View(new ResendCommand { UserId = id, ReturnUrl = returnUrl });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resend(ResendCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.UserId))
                {
                    TempData["message"] = "";
                    return View(command);
                }
            
                var response = await Mediator.Send(command);
            
                TempData["message"] = response.Message;
            
                return View(command);
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }
        
        public async Task<IActionResult> Confirm(string id, string token, string returnUrl)
        {
            var response = await Mediator.Send(new ConfirmCommand { UserId = id, Token = token });

            TempData["message"] = response.Message;
            
            return View(new ConfirmCommand { UserId = id, Token = token, ReturnUrl = returnUrl });
        }

        public IActionResult Recovery(string returnUrl)
        {
            return View(new RecoveryCommand { ReturnUrl = returnUrl});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recovery(RecoveryCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
            
                TempData["message"] = response.Message;
            
                if(response.Success)
                    return RedirectToAction("SignIn", "Account", new { command.ReturnUrl });

                return View(command);
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }
        
        public IActionResult Reset(string id, string token, string returnUrl)
        {
            return View(new ResetCommand() { UserId = id, Token = token, ReturnUrl = returnUrl });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(ResetCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                if (!response.Success)
                    return View(new ResetCommand
                        { UserId = command.UserId, Token = command.Token, ReturnUrl = command.ReturnUrl });

                TempData["message"] = response.Message;

                if (!string.IsNullOrEmpty(command.ReturnUrl))
                    return Redirect(command.ReturnUrl);

                return RedirectToAction("SignIn", "Account", new { command.ReturnUrl });
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }
        
        [HttpGet]
        public IActionResult TwoStep(string id, string returnUrl)
        {
            if (User.Identity is not {IsAuthenticated: true}) 
                return View(nameof(TwoStep), new TwoFactorCommand { Id =  Guid.Parse(id), ReturnUrl = returnUrl });
            
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public async Task<IActionResult> ResendOpt(string id, string returnUrl)
        {
            if (User.Identity is not {IsAuthenticated: true})
            {
                var response = await Mediator.Send(new OtpCommand { UserId = Guid.Parse(id), ReturnUrl = returnUrl });
            
                TempData["message"] = response.Message;
                
                return View(nameof(TwoStep), new TwoFactorCommand { Id =  Guid.Parse(id), ReturnUrl = returnUrl });
            }
            
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoStep(TwoFactorCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
            
                TempData["message"] = response.Message;
            
                if (!response.Success) 
                    return View(command);
            
                if (!string.IsNullOrEmpty(command.ReturnUrl))
                    return Redirect(command.ReturnUrl);
            
                return RedirectToAction("Index", "Home");
            }
            catch (ValidationException e)
            {
                CatchErrorValidation(e);
            }
            catch (ErrorInvalidException e)
            {
                CatchErrorInvalid(e);
            }
            
            return View(command);
        }

        private void CatchErrorValidation(ValidationException e)
        {
            Console.WriteLine(e);
                
            foreach(var failure in e.Errors)
            {
                ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
            }
        }
        
        private void CatchErrorInvalid(ErrorInvalidException e)
        {
            Console.WriteLine(e);
                
            e.Errors?.ToList().ForEach(x => ModelState.AddModelError("", x));
        }
    }
}