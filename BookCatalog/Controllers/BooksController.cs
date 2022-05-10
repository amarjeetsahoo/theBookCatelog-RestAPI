using BookCatalog.Dtos;
using BookCatalog.Models;
using BookCatalog.Repo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private IBook _BookRepo;
        public BooksController(IBook bookRepo)
        {
            _BookRepo = bookRepo;
           // _BookRepo = new InMemBookRepo();
        }
        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>>GetBooks()
        {
            return _BookRepo.GetBooks()
                .Select(x => new BookDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price
                }).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(Guid id)
        {
            var book = _BookRepo.GetBook(id);
            if (book == null)
                return NotFound();
            var bookDTO = new BookDTO
            { Id = book.Id, Title = book.Title, Price = book.Price };
            return bookDTO;
        }

        [HttpPost]
        public ActionResult CreateBook(CreateOrUpdateDTO book)
        {
            var mybook = new Book();
            mybook.Id = Guid.NewGuid();
            mybook.Title = book.Title;
            mybook.Price = book.Price;

            _BookRepo.CreateBook(mybook);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateBook(Guid id, CreateOrUpdateDTO book)
        {
            var mybook = _BookRepo.GetBook(id);

            if (mybook == null)
                return NotFound();

            mybook.Title = book.Title;
            mybook.Price = book.Price;

            _BookRepo.UpdateBook(id, mybook);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(Guid id)
        {
            var mybook = _BookRepo.GetBook(id);

            if (mybook == null)
                return NotFound();

            _BookRepo.DeleteBook(id);

            return Ok();
        }
    }
}
