using KhumaloCrafts_Part2.Models;
using KhumaloCrafts_Part2.Models.DTOs;
using KhumaloCrafts_Part2.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KhumaloCrafts_Part2.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IFileService _fileService;

    public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IFileService fileService)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetProducts();
        return View(products);
    }

    public async Task<IActionResult> AddProduct()
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
        });
        ProductDTO productToAdd = new() { CategoryList = categorySelectList };
        return View(productToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
        });
        productToAdd.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(productToAdd);

        try
        {
            if (productToAdd.ImageFile != null)
            {
                if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                string imageName = await _fileService.SaveFile(productToAdd.ImageFile, allowedExtensions);
                productToAdd.Image = imageName;
            }
            // manual mapping of ProductDTO -> Product
            Product product = new()
            {
                Id = productToAdd.Id,
                ProductName = productToAdd.ProductName,
                ArtisanName = productToAdd.ArtisanName,
                Description = productToAdd.Description,
                Image = productToAdd.Image,
                CategoryId = productToAdd.CategoryId,
                Price = productToAdd.Price
            };
            await _productRepo.AddProduct(product);
            TempData["successMessage"] = "Product is added successfully";
            return RedirectToAction(nameof(AddProduct));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToAdd);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToAdd);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(productToAdd);
        }
    }

    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _productRepo.GetProductById(id);
        if (product == null)
        {
            TempData["errorMessage"] = $"Product with the id: {id} does not found";
            return RedirectToAction(nameof(Index));
        }
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
            Selected = category.Id == product.CategoryId
        });
        ProductDTO productToUpdate = new()
        {
            CategoryList = categorySelectList,
            ProductName = product.ProductName,
            ArtisanName = product.ArtisanName,
            Description = product.Description,
            CategoryId = product.CategoryId,
            Price = product.Price,
            Image = product.Image
        };
        return View(productToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
            Selected = category.Id == productToUpdate.CategoryId
        });
        productToUpdate.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(productToUpdate);

        try
        {
            string oldImage = "";
            if (productToUpdate.ImageFile != null)
            {
                if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                string imageName = await _fileService.SaveFile(productToUpdate.ImageFile, allowedExtensions);
                // hold the old image name. Because we will delete this image after updating the new
                oldImage = productToUpdate.Image;
                productToUpdate.Image = imageName;
            }
            // manual mapping of ProductDTO -> Product
            Product product = new()
            {
                Id = productToUpdate.Id,
                ProductName = productToUpdate.ProductName,
                ArtisanName = productToUpdate.ArtisanName,
                Description = productToUpdate.Description,
                CategoryId = productToUpdate.CategoryId,
                Price = productToUpdate.Price,
                Image = productToUpdate.Image
            };
            await _productRepo.UpdateProduct(product);
            // if image is updated, then delete it from the folder too
            if (!string.IsNullOrWhiteSpace(oldImage))
            {
                _fileService.DeleteFile(oldImage);
            }
            TempData["successMessage"] = "Product is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToUpdate);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToUpdate);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(productToUpdate);
        }
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _productRepo.GetProductById(id);
            if (product == null)
            {
                TempData["errorMessage"] = $"Product with the id: {id} does not found";
            }
            else
            {
                await _productRepo.DeleteProduct(product);
                if (!string.IsNullOrWhiteSpace(product.Image))
                {
                    _fileService.DeleteFile(product.Image);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on deleting the data";
        }
        return RedirectToAction(nameof(Index));
    }

}