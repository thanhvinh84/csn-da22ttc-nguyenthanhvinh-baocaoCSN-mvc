namespace Shopping_Tutorial.Models
{
    public class CartItemModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        // Thêm thuộc tính này để lưu giá trị Price dưới dạng chuỗi
        public string PriceString
        {
            get { return Price.ToString("0.##"); } // Có thể tùy chỉnh định dạng chuỗi cho phù hợp
        }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }
        public CartItemModel()
        {

        }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            // Kiểm tra nếu product.Price là kiểu double hoặc float, chuyển nó thành decimal
            Price = Convert.ToDecimal(product.Price); // Hoặc (decimal)product.Price;
            Quantity = 1;
            Image = product.Image;
        }
    }
}