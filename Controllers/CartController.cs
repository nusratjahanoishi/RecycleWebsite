using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Helpers;
using NextUses.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class CartController : Controller
{
    private readonly NextUsesDB _context;

    public CartController(NextUsesDB context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new List<CartItem>();
        return View(cart);
    }

    public IActionResult AddToCart(Guid id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new List<CartItem>();

        var existing = cart.FirstOrDefault(c => c.ProductId == id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price ?? 0,   // ✅ fixes decimal? to decimal issue
                Quantity = 1,
                Image = product.Image  // ✅ only if Product has ImageUrl, otherwise remove this line
            });
        }

        HttpContext.Session.SetObject("cart", cart);
        return RedirectToAction("Index");
    }

    public IActionResult Remove(Guid id)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new List<CartItem>();
        var item = cart.FirstOrDefault(c => c.ProductId == id);
        if (item != null) cart.Remove(item);

        HttpContext.Session.SetObject("cart", cart);
        return RedirectToAction("Index");
    }

    public IActionResult Clear()
    {
        HttpContext.Session.Remove("cart");
        return RedirectToAction("Index");
    }
}
