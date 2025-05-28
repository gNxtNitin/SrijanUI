﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UserManagementService.IRepository;
using UserManagementService.Models;


namespace SRIJANWEBUI.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuRepository _menuRepository;

        public MenuViewComponent(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {


            string userMenu = HttpContext.Session.GetString("UserMenu");

            if (userMenu == null || userMenu == "[]")
            {
            //var tt = HttpContext.User.Claims.Where(x => x == ClaimTypes.NameIdentifier).FirstOrDefault();

                string userId = HttpContext.User.FindFirst("UserId")?.Value;
                var menuRes = await _menuRepository.GetMenuByUserId(userId);
                HttpContext.Session.SetString("UserMenu", JsonSerializer.Serialize(menuRes));
                return View("Menu", menuRes);
            }

            List<MenuMaster> menuItems = JsonSerializer.Deserialize<List<MenuMaster>>(userMenu).OrderBy(x => x.Order).ToList();

            return View("Menu", menuItems);
        }
    }
}
