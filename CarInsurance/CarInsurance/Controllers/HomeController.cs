using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers;

public class InsureeController : Controller
{
    private InsuranceEntities db = new InsuranceEntities();

    // GET: Insuree/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Insuree/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            insuree.Quote = CalculateQuote(insuree);
            db.Insurees.Add(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(insuree);
    }

    private decimal CalculateQuote(Insuree insuree)
    {
        decimal quote = 50; // Base rate

        // Age-based calculations
        if (insuree.Age <= 18)
            quote += 100;
        else if (insuree.Age >= 19 && insuree.Age <= 25)
            quote += 50;
        else if (insuree.Age > 25)
            quote += 25;

        // Car year calculations
        if (insuree.CarYear < 2000)
            quote += 25;
        else if (insuree.CarYear > 2015)
            quote += 25;

        // Car make and model calculations
        if (insuree.CarMake.Equals("Porsche", StringComparison.OrdinalIgnoreCase))
        {
            quote += 25; // Base for Porsche
            if (insuree.CarModel.Equals("911 Carrera", StringComparison.OrdinalIgnoreCase))
                quote += 25; // Additional for 911 Carrera
        }

        // Speeding tickets
        quote += 10 * insuree.SpeedingTickets;

        // DUI adjustment
        if (insuree.DUI)
            quote *= 1.25m; // Add 25%

        // Full coverage adjustment
        if (insuree.FullCoverage)
            quote *= 1.5m; // Add 50%

        return quote;
    }
}