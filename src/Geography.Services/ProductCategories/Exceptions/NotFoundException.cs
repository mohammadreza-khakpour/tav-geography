
using Geography.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.ProductCategories.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public override string Message => "NotFoundCategory";

        public override string ToString()
        {
            return "No commodity groups were found for change";
        }
    }
}
