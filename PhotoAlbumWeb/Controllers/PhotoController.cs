using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAlbumWeb.Data;
using PhotoAlbumWeb.Models;

namespace PhotoAlbumWeb.Controllers
{
    public class PhotoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public PhotoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile imageFile, string photoDescription, 
            string eventDescription ,string eventLocation ,DateTime eventDate)
        {
       
            //Verified photo
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Please select a image.");
                return View();
            }

            //
            else if (photoDescription.Length < 25 || photoDescription.Length >100)
            {
                ModelState.AddModelError("photoDescription", "The photo description field must have a minimum " +
                    "of 25 characters and a maximum of 100 characters.");
            }

            //
            else if (eventDescription.Length < 25 || eventDescription.Length > 100)
            {
                ModelState.AddModelError("eventDescription", "The photo description field must have a minimum " +
                   "of 25 characters and a maximum of 100 characters.");
            }

            //
            else if (eventLocation.Length <= 0)
            {
                ModelState.AddModelError("eventLocation", "The location field is empty");
            }

            //
            else if (eventDate < DateTime.Today)
            {
                ModelState.AddModelError("eventDate", "The event date cannot be in the past.");
            }

            //Valited to model 
            if (!ModelState.IsValid)
            {
                return View();
            }


            //
            byte[] photoBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                photoBytes = memoryStream.ToArray();
            }

            //
            var photoBoard = new PhotoBoard
            {
                Photo = photoBytes,
                PhotoDescription = photoDescription,
                EventDescription = eventDescription,
                Location = eventLocation,
                EventDate = eventDate
            };

            //
             _appDBContext.PhotoBoard.Add(photoBoard);
            await _appDBContext.SaveChangesAsync();

            //
            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Retrieves all records from the database
            var photoBoards = await _appDBContext.PhotoBoard.ToListAsync();

            // Returns the list to the view
            return View(photoBoards);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var photoBoard = await _appDBContext.PhotoBoard.FindAsync(id);
            if (photoBoard == null)
            {
                return NotFound();
            }

            _appDBContext.PhotoBoard.Remove(photoBoard);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var photoBoard = await _appDBContext.PhotoBoard.FindAsync(id);

            if (photoBoard == null)
            {
                return NotFound();
            }

            return View(photoBoard);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile? imageFile, PhotoBoard model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Returns the view if there are validation errors
            }

            var existingPhotoBoard = await _appDBContext.PhotoBoard.FindAsync(id);
            if (existingPhotoBoard == null)
            {
                return NotFound();
            }

            // Actualiza las propiedades básicas
            existingPhotoBoard.PhotoDescription = model.PhotoDescription;
            existingPhotoBoard.EventDescription = model.EventDescription;
            existingPhotoBoard.Location = model.Location;
            existingPhotoBoard.EventDate = model.EventDate;

            // Image file handling
            if (imageFile != null && imageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                existingPhotoBoard.Photo = memoryStream.ToArray();
            }

            // Save changes to the database
            _appDBContext.PhotoBoard.Update(existingPhotoBoard);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}
