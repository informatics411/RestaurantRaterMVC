using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Restaurant;
using RestaurantRaterMVC.Services;

namespace RestaurantRaterMVC.Controllers
{
    public class RestaurantController : Controller
    {
        private IRestaurantService _service;
        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() 
        {
            List<RestaurantListItem> restaurants = await _service.GetAllRestaurants();
            return View(restaurants);
        }

        [ActionName("Details")]
        public async Task<IActionResult> Restaurant(int id)
        //  List<RestaurantListItem> restaurants = await _service.GetAllRestaurants();
        //  return View(restaurants);
        {
            Restaurant restaurant = await _service.GetAllRestaurants()
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return RedirectToAction(nameof(Index));
        

             RestaurantDetail restaurantDetail = new RestaurantDetail()
             {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location,
                Score = restaurant.Score
             };
             return View(restaurantDetail);   
        }


        public async Task<IActionResult> Create (int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantCreate model)
        {
            if (!ModelState.IsValid)
            return View(model);
            await _service.CreateRestaurant(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            return RedirectToAction(nameof(Index));

            RestaurantEdit restaurantEdit = new RestaurantEdit()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location
            };
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RestaurantEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            return RedirectToAction(nameof(Index));

            restaurant.Name = model.Name;
            restaurant.Location = model.Location;
            await _model.SaveChangesAsync();
            return RedirectToAction("Details", new { id = restaurant.Id });


        }

    }
}