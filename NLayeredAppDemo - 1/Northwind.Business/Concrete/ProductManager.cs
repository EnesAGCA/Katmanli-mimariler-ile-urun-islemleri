using Northwind.Business.Abstract;
using Northwind.Business.ValidationRules.FluentValidation;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Northwind.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private EfCategoryDal efCategoryDal;

        

        public ProductManager(IProductDal productDal) 
        {
            _productDal = productDal;
        }

        public ProductManager(EfCategoryDal efCategoryDal)
        {
            this.efCategoryDal = efCategoryDal;
        }

        public void Add(Product product)
        {
            ProductValidator productValidator = new ProductValidator();
            var result = productValidator.Validate(product);
            if (result.Errors.Count > 0)      
            {
                throw new ValidationException(result.Errors);       
            }
            _productDal.Add(product);
        }

        public void Delete(Product product)
        {
            try            
            {
                _productDal.Delete(product);
            }
            catch
            {
                throw new Exception("Silme Gerçekleşemedi");       

            }
        }

        public List<Product> GetAll()
        {
            //Business code
            return _productDal.GetAll();
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _productDal.GetAll(p => p.CategoryId == categoryId);
        }

        public List<Product> GetProductsByProductName(string productName)
        {
            return _productDal.GetAll(p => p.ProductName.ToLower().Contains(productName.ToLower()));
        }

        public void Update(Product product)
        {

            _productDal.Update(product);
        }
    }
}

