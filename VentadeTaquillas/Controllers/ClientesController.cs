﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Data;
using VentadeTaquillas.ViewModels;

namespace VentadeTaquillas.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {

            var result = (await _context.Taquillas.Select(s => new VMClienteR
            {
                ClienteId = s.ClienteId,
                Nombre = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Nombre,
                Apellido = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Apellido,
                PeliculaNombre = _context.Peliculas.Where(c => c.PeliculaId == s.PeliculaId).FirstOrDefault().NombrePeli,
                Correo = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Correo,
                Ciudad = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Ciudad,
                Telefono = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Telefono,

            }).ToListAsync());

            return View(result);
        }

        // GET: Clientes/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {


            return View();
        }


        public IActionResult Intermedio()
        {

            int Idclientes = _context.Clientes.Max(p => p.NumeroClienteId);

            Guid? Idcliente = _context.Clientes.Where(p=>p.NumeroClienteId==Idclientes).FirstOrDefault().ClienteId;
            Guid? IdPelicula = _context.Clientes.Where(c => c.ClienteId == Idcliente).FirstOrDefault().PeliculaId;

            ViewBag.idCliente = Idcliente;
            ViewBag.idPelicula = IdPelicula;


            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,PeliculaId,Nombre,Apellido,Usuario,Correo,Ciudad,Telefono")] Cliente cliente, Guid? id)
        {
            if (ModelState.IsValid)
            {
                cliente.ClienteId = Guid.NewGuid();
                cliente.PeliculaId = ViewBag.idpeli = id;
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction("Intermedio","Clientes");
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid? idcl, Guid? id)
        {
            if (idcl == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(idcl);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClienteId,Nombre,Apellido,Usuario,Correo,Ciudad,Telefono")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Intermedio", "Clientes");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
