internal class Program
{
    private static void Main(string[] args)
    {
                // Func<order, double> discountcalculator1 =discountForHighPriceOrder;
                // Func<order, double> discountcalculator2 = discountForNewOrder;
                // Func<order, double> discountcalculator3 = discountForBigorder;


                // Func<order, bool> creteria1 =IsHighPriceProduct;
                // Func<order, bool> creteria2 =IsNewProduct;
                // Func<order, bool> creteria3= IsBigQuantity;

                
                    
                (Func<order, bool>  creteria,  Func<order, double> calculator)  rule1 =  (IsHighPriceProduct , discountForHighPriceOrder);
                (Func<order, bool> creteria, Func<order, double> calculator)  rule2 = (IsNewProduct , discountForNewOrder);
                (Func<order, bool> creteria, Func<order, double> calculator)  rule3 =  (IsBigQuantity , discountForBigorder);

                var rules = new List<(Func<order, bool> , Func<order, double>)>{rule1 , rule2 , rule3};

                Func<order,List<(Func<order, bool> , Func<order, double>)>, List<Func<order, double>>> calculators = GetCalculators;

                List<order> orders = new List<order> {
                    new order{
                        Product= new Product{ProductId=1 , UnitPrice = 10},
                        Quantity = 2,
                        ExpirationDate = new DateTime(2023,2,5)
                    },
                    new order{
                        Product= new Product{ProductId=2 , UnitPrice = 4},
                        Quantity = 3,
                        ExpirationDate = new DateTime(2023,2,15)
                    },
                    new order{
                        Product= new Product{ProductId=3 , UnitPrice = 6},
                        Quantity = 1,
                        ExpirationDate = new DateTime(2023,2,20)
                    },
                };

        var discounts = orders.Select(x=>  
                calculators(x , rules).Select(y=>  y(x)).Average()
        );

        Console.WriteLine("Hello, World!");
         Console.WriteLine(discounts.First());
    }
    
        public static List<Func<order, double>> GetCalculators(order order , List<(Func<order, bool> creteria , Func<order, double> calculator)> rules){
            return rules.Where(r=> r.creteria(order)).Select(r=>r.calculator).Take(3).ToList();
        }



        public static bool IsHighPriceProduct(order order)
        {
            return order.Product.UnitPrice > 5;
        }

        public static bool IsNewProduct(order order)
        {
            return order.ExpirationDate > DateTime.Now;
        }

        public static bool IsBigQuantity(order order)
        {
            return order.Quantity > 2;
        }

        public static double discountForHighPriceOrder(order order){
            return order.Quantity * order.Product.UnitPrice  - 1;
        } 

        public static double discountForNewOrder(order order){
            return order.Quantity * order.Product.UnitPrice - 0.5;
        } 
        public static double discountForBigorder(order order){
            return order.Quantity * order.Product.UnitPrice  - 2;
        } 
}


public class Product {
    public int ProductId { get; set; }
    public double UnitPrice { get; set; }
}
public class order {
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
}