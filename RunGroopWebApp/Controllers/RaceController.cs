﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _context;
        public RaceController(IRaceRepository context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _context.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Details(int id)
        {
            Race race = await _context.GetRaceById(id);
            return View(race);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if(!ModelState.IsValid) 
                return View(race);
            _context.Add(race);
            return RedirectToAction("Index");
        }
    }
}
