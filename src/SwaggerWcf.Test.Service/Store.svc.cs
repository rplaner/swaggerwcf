﻿using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using SwaggerWcf.Attributes;
using SwaggerWcf.Test.Service.Data;

namespace SwaggerWcf.Test.Service
{
    [SwaggerWcf("/v1/rest")]
    public class BookStore : IStore
    {
        #region /books

        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book title)", true)]
        public Book CreateBook(Book value)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            if (value == null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return null;
            }

            if (!string.IsNullOrWhiteSpace(value.Title) && value.Title == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            value.Id = Guid.NewGuid().ToString("N");
            Store.Books.Add(value);

            woc.OutgoingResponse.StatusCode = HttpStatusCode.Created;

            return value;
        }

        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.OK, "Books found, values in the response body")]
        [SwaggerWcfResponse(HttpStatusCode.NotFound, "Book not found", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book id)", true)]
        public Book ReadBook(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            Book book = Store.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return book;
            }

            if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            return null;
        }

        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.OK, "Book found, value in the response body")]
        [SwaggerWcfResponse(HttpStatusCode.NoContent, "No books", true)]
        public Book[] ReadBooks()
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            if (Store.Books.Any())
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return Store.Books.ToArray();
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            return null;
        }

        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.NoContent, "Book updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request")]
        [SwaggerWcfResponse(HttpStatusCode.NotFound, "Book not found")]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book id)")]
        public void UpdateBook(string id, Book value)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return;

            if (value == null || value.Id != id)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return;
            }

            Book book = Store.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                Store.Books.Remove(book);
                Store.Books.Add(value);
            }
            else if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            else
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            }
        }

        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.NoContent, "Book deleted")]
        [SwaggerWcfResponse(HttpStatusCode.NotFound, "Book not found")]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book id)")]
        public void DeleteBook(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return;

            Book book = Store.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                Store.Books.Remove(book);
            }
            else if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            else
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            }
        }

        [SwaggerWcfTag("LowPerformance", true)]
        [SwaggerWcfTag("Books")]
        [SwaggerWcfResponse(HttpStatusCode.OK, "Book found, author value in the response body")]
        [SwaggerWcfResponse(HttpStatusCode.NoContent, "Book found, author value in the response body")]
        [SwaggerWcfResponse(HttpStatusCode.NotFound, "Book not found", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book id)", true)]
        public Author ReadBookAuthor(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            Book book = Store.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                if (book.Author == null)
                {
                    woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                    return null;
                }

                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return book.Author;
            }

            if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            return null;
        }

        #endregion

        #region /authors

        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(201, "Author created, value in the response body with id updated")]
        [SwaggerWcfResponse(400, "Bad request", true)]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as author name)", true)]
        public Author CreateAuthor(Author value)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            if (value == null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return null;
            }

            if (!string.IsNullOrWhiteSpace(value.Name) && value.Name == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            value.Id = Guid.NewGuid().ToString("N");
            Store.Authors.Add(value);

            woc.OutgoingResponse.StatusCode = HttpStatusCode.Created;

            return value;
        }

        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(200, "Author found, value in the response body")]
        [SwaggerWcfResponse(404, "Author not found", true)]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as author id)", true)]
        public Author ReadAuthor(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            Author author = Store.Authors.FirstOrDefault(b => b.Id == id);
            if (author != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return author;
            }

            if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            return null;
        }

        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(200, "Authors found, values in the response body")]
        [SwaggerWcfResponse(204, "No authors", true)]
        public Author[] ReadAuthors()
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            if (Store.Authors.Any())
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return Store.Authors.ToArray();
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            return null;
        }

        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(204, "Author updated")]
        [SwaggerWcfResponse(400, "Bad request")]
        [SwaggerWcfResponse(404, "Author not found")]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as author id)")]
        public void UpdateAuthor(string id, Author value)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return;

            if (value == null || value.Id != id)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return;
            }

            Author author = Store.Authors.FirstOrDefault(b => b.Id == id);
            if (author != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                Store.Authors.Remove(author);
                Store.Authors.Add(value);
            }
            else if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            else
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            }
        }

        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(204, "Author deleted")]
        [SwaggerWcfResponse(404, "Author not found")]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as author id)")]
        public void DeleteAuthor(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return;

            Author author = Store.Authors.FirstOrDefault(b => b.Id == id);
            if (author != null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                Store.Authors.Remove(author);
            }
            else if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            else
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            }
        }

        [SwaggerWcfTag("LowPerformance", true)]
        [SwaggerWcfTag("Authors")]
        [SwaggerWcfResponse(200, "Book found, author value in the response body")]
        [SwaggerWcfResponse(204, "Book found, author value in the response body")]
        [SwaggerWcfResponse(404, "Book not found", true)]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as book id)", true)]
        public Book[] ReadAuthorBooks(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            Author author = Store.Authors.FirstOrDefault(b => b.Id == id);
            if (author != null)
            {
                Book[] books = Store.Books.Where(b => b.Author.Id == id).ToArray();
                if (!books.Any())
                {
                    woc.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                    return null;
                }

                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return books;
            }

            if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            return null;
        }

        #endregion

        #region /languages
        
        [SwaggerWcfHidden]
        [SwaggerWcfTag("Languages")]
        [SwaggerWcfResponse(201, "Language created, value in the response body with id updated")]
        [SwaggerWcfResponse(400, "Bad request", true)]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as language name)", true)]
        public Language CreateLanguage(Language value)
        {
            throw new NotImplementedException();
        }
        
        [SwaggerWcfHidden]
        [SwaggerWcfTag("Languages")]
        [SwaggerWcfResponse(200, "Language found, value in the response body")]
        [SwaggerWcfResponse(404, "Language not found", true)]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as language id)", true)]
        public Language ReadLanguage(string id)
        {
            throw new NotImplementedException();
        }

        [SwaggerWcfTag("Languages")]
        [SwaggerWcfResponse(200, "Language found, value in the response body")]
        [SwaggerWcfResponse(204, "No languages", true)]
        public Language[] ReadLanguages()
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;
            
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return Enum.GetValues(typeof(Language)).Cast<Language>().ToArray();
        }
        
        [SwaggerWcfHidden]
        [SwaggerWcfTag("Languages")]
        [SwaggerWcfResponse(204, "Language updated")]
        [SwaggerWcfResponse(400, "Bad request")]
        [SwaggerWcfResponse(404, "Language not found")]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as language id)")]
        public void UpdateLanguage(string id, Language value)
        {
            throw new NotImplementedException();
        }
        
        [SwaggerWcfHidden]
        [SwaggerWcfTag("Languages")]
        [SwaggerWcfResponse(204, "Language deleted")]
        [SwaggerWcfResponse(404, "Language not found")]
        [SwaggerWcfResponse(500, "Internal error (can be forced using ERROR_500 as language id)")]
        public void DeleteLanguage(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
