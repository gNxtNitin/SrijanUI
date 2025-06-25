using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SRIJANWEBUI.Models;
using SRIJANWEBUI.Utility;
using SRIJANWEBUI.Utility;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using UserManagementService.DTOs.RequestModels;
using UserManagementService.IRepository;
using UserManagementService.Models;
using UserManagementService.Utility;

namespace SRIJANWEBUI.Controllers
{
    public class AuthController : Controller
    {
        private IAccountRepository _accountRepository;
        private IMemoryCacheService _memoryCacheService;
        private readonly ICustomerRepository _customerRepository;

        public AuthController(IAccountRepository accountRepository, IMemoryCacheService memoryCacheService, ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _memoryCacheService = memoryCacheService;
            _customerRepository = customerRepository;
        }
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        public async Task<IActionResult> LogIn()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User";

                if (role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase))
                {

                    return RedirectToAction("Orders", "Customer");
                }

            }

            if (Request.Cookies.TryGetValue("LogoutMessage", out var message))
            {
                ViewData["LoginErrorMessage"] = message;

                Response.Cookies.Delete("LogoutMessage");
            }

            try
            {
                var companies = await _customerRepository.GetCompany();
                ViewBag.Companies = companies;
            }
            catch (Exception ex)
            {
                ViewBag.Companies = null;
            }
           
            return View(new UserLogin());
        }
        public async Task<IActionResult> LogInAdmin()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";

                if (!role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase))
                {
                    switch (role)
                    {
                        case "ADMIN":
                            // Code block
                            return RedirectToAction("Dashboard", "Admin");
                        //break;

                        case "USER":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        case "HOD":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        case "INCHARGE":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        default:
                            // Code block
                            return RedirectToAction("LoginAdmin", "Auth");
                    }

                }

            }

            if (Request.Cookies.TryGetValue("LogoutMessage", out var message))
            {
                ViewData["LoginErrorMessage"] = message;

                Response.Cookies.Delete("LogoutMessage");
            }

            //try
            //{
            //    var companies = await _customerRepository.GetCompany();
            //    ViewBag.Companies = companies;
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Companies = null;
            //}

            return View(new AdminLogin());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInAdmin(AdminLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    ViewData["LoginErrorMessage"] = "Invalid input.";
                    return View(userLogin);
                }


                //var kk = PasswordConfig.GetMd5Hash(userLogin.Password);
                LoginRequest loginRequest = new LoginRequest()
                {
                    MobileOrEmail = userLogin.EmpId,
                    CompanyCode = 0,
                    UserId = string.Empty,
                    Password = userLogin.Password,
                    IsJwtToken = true,
                    IsResendCode = 0,
                    IsLoginWithOtp = false
                };

                LoginStatus loginStatus = await _accountRepository.LoginAdmin(loginRequest);
                if (loginStatus.Success)
                {
                    

                    string role = loginStatus.Role;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginStatus.UserId.ToString()),

                        new Claim(ClaimTypes.Role, role),
                        new Claim("CustomerType", role),
                        new Claim("UserId", loginStatus.UserId),
                        new Claim("UserName", loginStatus.UserName),
                        //new Claim("CompanyName", loginStatus.CompanyName),
                        new Claim("EmpId", loginStatus.EmpId),
                        //new Claim("CompanyCode", userLogin.CompanyCode.ToString())

                    };


                    //if (loginStatus.PasswordState == PasswordStatus.AboutToExpire)
                    //{
                    //    TempData["PwExipreAlert"] = loginStatus.Message;
                    //}
                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in user with cookie
                    await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
                    {
                        IsPersistent = false
                    });

                    var tokenCookieOptions = new CookieOptions()
                    {
                        HttpOnly = true,
                        //Secure = true,
                        //SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(4)
                    };

                    //HttpContext.Session.SetString("UserName", loginStatus.UserName);
                    // HttpContext.Session.SetString("CompanyName", loginStatus.CompanyName);
                    HttpContext.Response.Cookies.Append("user_token", loginStatus.Token, tokenCookieOptions);
                    // TODO - Change token handling (do not store in the session)
                    //HttpContext.Session.SetString("user_token", loginStatus.Token);
                    //HttpContext.Session.SetInt32("userId", loginStatus.UserId);
                    if(loginStatus.RespCode == 201)
                    {
                        return RedirectToAction("ChangePassword", "Auth");
                    }


                    switch (role)
                    {
                        case "ADMIN":
                            // Code block
                            return RedirectToAction("Dashboard", "Admin");
                            //break;

                        case "USER":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        case "HOD":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        case "INCHARGE":
                            // Code block
                            return RedirectToAction("MyPunchingReport", "Report");
                        default:
                            // Code block
                            return RedirectToAction("LoginAdmin", "Auth");
                    }

                    //return role.Equals("C", StringComparison.InvariantCultureIgnoreCase) ? RedirectToAction("Index", "Customer") : RedirectToAction("Dashboard", role, new { area = role });
                }
                else
                {
                    
                    ViewData["LoginErrorMessage"] = "Invalid Employee ID or Password";
                    return View(userLogin);
                }
            }
            catch (Exception ex)
            {
                
                ViewData["LoginErrorMessage"] = "Something went wrong!";
                return View(userLogin);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var companies = await _customerRepository.GetCompany();
                    ViewBag.Companies = companies;
                    ViewData["LoginErrorMessage"] = "Invalid input.";
                    return View(userLogin);
                }

                ////if using captcha service
                //string captchaKey = HttpContext.Session.GetString("CaptchaVerified") ?? "";

                //if (string.IsNullOrEmpty(captchaKey))
                //{
                //    ViewData["LoginErrorMessage"] = "Invalid/expired captcha!";
                //    return View(userLogin);
                //}
                //else if (!captchaKey.Equals(userLogin.CaptchaKey, StringComparison.OrdinalIgnoreCase))
                //{
                //    ViewData["LoginErrorMessage"] = "please verify captcha!";
                //    return View(userLogin);
                //}
                //else
                //{
                //    HttpContext.Session.Remove("CaptchaVerified");
                //}


                LoginRequest loginRequest = new LoginRequest()
                {
                    MobileOrEmail = userLogin.CustomerId,
                    CompanyCode = userLogin.CompanyCode,
                    UserId = string.Empty,
                    Password = userLogin.Password,
                    IsJwtToken = true,
                    IsResendCode = 0,
                    IsLoginWithOtp = false
                };

                LoginStatus loginStatus = await _accountRepository.LoginUser(loginRequest);
                if (loginStatus.Success)
                {
                    //if (loginStatus.PasswordState == PasswordStatus.Expired)
                    //{

                    //    var isSent = await _accountRepository.SendForgotPasswordEmail(loginRequest.MobileOrEmail);
                    //    if (isSent)
                    //    {
                    //        ViewData["LoginErrorMessage"] = "Your Password Has been expired. Please reset the password from url sent in Email";
                    //    }
                    //    else
                    //    {
                    //        ViewData["LoginErrorMessage"] = "Your Password Has been expired, Failed to send reset link in email, try again";
                    //    }

                    //    return View(userLogin);
                    //}


                    //if (loginStatus.IsOtpRequired)
                    //{
                    //    TempData["IsToken"] = true;
                    //    TempData["UserId"] = loginStatus.UserId;

                    //    if (loginStatus.PasswordState == PasswordStatus.AboutToExpire)
                    //    {
                    //        TempData["PwExipreAlert"] = loginStatus.Message;
                    //    }

                    //    return RedirectToAction("ValidateOTP", new { uid = loginStatus.UniqueId });
                    //}


                   

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginStatus.UserId.ToString()),

                        new Claim(ClaimTypes.Role, loginStatus.Role),
                        new Claim("CustomerType", loginStatus.Role),
                        new Claim("UserId", loginStatus.UserId),
                        new Claim("UserName", loginStatus.UserName),
                        new Claim("CompanyName", loginStatus.CompanyName),
                        new Claim("CustomerId", userLogin.CustomerId),
                        new Claim("CompanyCode", userLogin.CompanyCode.ToString())

                    };


                    //if (loginStatus.PasswordState == PasswordStatus.AboutToExpire)
                    //{
                    //    TempData["PwExipreAlert"] = loginStatus.Message;
                    //}
                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in user with cookie
                    await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
                    {
                        IsPersistent = false
                    });

                    var tokenCookieOptions = new CookieOptions()
                    {
                        HttpOnly = true,
                        //Secure = true,
                        //SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(4)
                    };
                    
                    //HttpContext.Session.SetString("UserName", loginStatus.UserName);
                   // HttpContext.Session.SetString("CompanyName", loginStatus.CompanyName);
                    HttpContext.Response.Cookies.Append("user_token", loginStatus.Token, tokenCookieOptions);
                    // TODO - Change token handling (do not store in the session)
                    //HttpContext.Session.SetString("user_token", loginStatus.Token);
                    //HttpContext.Session.SetInt32("userId", loginStatus.UserId);


                    return RedirectToAction("Orders", "Customer");
                }
                else
                {
                    var companies = await _customerRepository.GetCompany();
                    ViewBag.Companies = companies;
                    ViewData["LoginErrorMessage"] = "Bad Credentials!";
                    return View(userLogin);
                }
            }
            catch (Exception ex)
            {
                var companies = await _customerRepository.GetCompany();
                ViewBag.Companies = companies;
                ViewData["LoginErrorMessage"] = "Something went wrong!";
                return View(userLogin);
            }

        }

        public async Task<IActionResult> ResetPassword(string? authResetToken)
        {
            if (string.IsNullOrEmpty(authResetToken))
            {
                TempData["ForgotPasswordRequestError"] = "Reset Password Url Is Invalid/Expired";
                RedirectToAction("LoginAdmin");
            }


            (bool isRequestValid, string userId) = await _accountRepository.ResetPasswordUrlValidate(authResetToken);

            if (isRequestValid && !string.IsNullOrEmpty(userId))
            {
                return View(new ResetPasswordViewModel { UserId = EncDecHelper.Encrypt(userId) });
            }

            //toast message on login page
            TempData["ForgotPasswordRequestError"] = "Reset Password Url Is Invalid/Expired";
            return RedirectToAction("LoginAdmin");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetUserPasswordData)
        {
            if (!ModelState.IsValid)
            {
                // Validation failed – return view with errors
                return View(resetUserPasswordData);
            }
            resetUserPasswordData.UserId = EncDecHelper.Decrypt(resetUserPasswordData.UserId);
            bool isResetSuccess = await _accountRepository.ResetPassword(resetUserPasswordData.UserId, resetUserPasswordData.Password);
            if (isResetSuccess)
            {
                TempData["ResetPasswordSuccessMessage"] = "Password updated Successfully!";
                return RedirectToAction("LoginAdmin");
            }


            TempData["ResetPasswordErrorMessage"] = "Failed to reset password!";
            return RedirectToAction("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            ViewBag.ErCode = TempData["CPasswordCode"] as string;
            ViewBag.ErMsg = TempData["CPasswordMsg"] as string;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string userId, string password)
        {
            if (string.IsNullOrEmpty(password) || !IsValidPassword(password))
            {
                TempData["CPasswordCode"] = "0";
                TempData["CPasswordMsg"] = "Invalid Password";
                return RedirectToAction("ChangePassword");
                //return new JsonResult(new { code = -1, message = "Invalid Password. Password must be at least 8 characters long and contain at least one letter, one number, and one special character." });
            }
            bool res = await _accountRepository.UpdatePassword(userId, password);
            
            if (res)
            {
                TempData["CPasswordCode"] = "1";
                TempData["CPasswordMsg"] = "Password updated Successfully!";
            }
            else
            {
                TempData["CPasswordCode"] = "0";
                TempData["CPasswordMsg"] = "Password update failed";
            }


            return RedirectToAction("ChangePassword");
        }


        [HttpPost]
        public async Task<JsonResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                return new JsonResult(new { Status = -400 });
            }

            EmailStatus emailSentStatus = await _accountRepository.SendForgotPasswordEmail(email);


            return new JsonResult(emailSentStatus);
        }




        public async Task<IActionResult> Logout()
        {

            string role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            HttpContext.Session.Clear();
            Response.Cookies.Delete("user_token");
            await HttpContext.SignOutAsync("CookieAuth");

            if (role.Equals("CUSTOMER", StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToAction("Login", "Auth");
            }
            
            return RedirectToAction("LogInAdmin", "Auth");

        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }


       

    }
}
