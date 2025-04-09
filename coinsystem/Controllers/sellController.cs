using coinsystem.Services;
using coinsystem.Models;
using K4os.Compression.LZ4.Internal;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace coinsystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellController : ControllerBase
    {
        private readonly IMoneyService  _moneyService;
        private readonly IProductService  _productService;
        private readonly IMemberService  _memberService;

        public SellController(IMoneyService moneyService, IProductService productService,IMemberService memberService)
        {
             _productService = productService;
        
             _moneyService = moneyService;
             
             _memberService = memberService;
        }

        // [HttpPost("addTell")]
                    // public async Task<IActionResult> AddTell(string tell)
                    // {
                    //   try
                    //   {
                    //     Member member = new Member
                    //     {
                    //       Tell = tell,
                    //       Credit = 0
                    //     };
                    //     
                    //     var Result =  await _memberService.AddMemberAsync(member);
                    //     return Ok(Result);
                    //   }
                    //   catch (Exception a)
                    //   {
                    //     Console.WriteLine(a);
                    //     throw;
                    //   }
                    // }


        [HttpGet("getMember/{tell}")]
        public async Task<IActionResult> GetTell(string tell)
        {
            try
            {
                Console.WriteLine(tell);
                if (string.IsNullOrEmpty(tell))
                {
                    return BadRequest("Tell cannot be empty");
                }

                var member = await _memberService.GetMemberByTell(tell);

                if (member == null)
                {
                    var newMember = new Member
                    {
                        Tell = tell,
                        Credit = 0
                    };
                    var result = await _memberService.AddMemberAsync(newMember);
                    return Ok(new { Credit = result.Credit });
                }

                return Ok(new { Credit = member.Credit });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("sell")]
        public async Task<IActionResult> SellProduct([FromQuery] int producid, [FromBody] List<MoneyDTO> money,string tell)
        {
          try
          {
            
            // if (money != null && money.Count > 0)
            // {
            //   
            // }
            var Member = await _memberService.GetMemberByTell(tell);
            
            Console.WriteLine("tell == "+ tell);
            if (tell == null || money == null)
            {
              return BadRequest("Tell is required");
            }
            // if()
            // if (money == null || money.Count == 0)
            // {
            //     return BadRequest("Money list is required");
            // }
            
  

            var product =  await _productService.GetProductByIdAsync(producid);
            if (product.Amount == 0)
            {
                return BadRequest(new { message = "Product is out of stock", money });
            }

            if (Member.Credit < product.Price)
            {
              return BadRequest(new { message = "Not enough credit " });
            }

            if (Member.Credit == 0)
            {
              if (money == null)
              {
                return BadRequest(new { message = "Money list is required" });
              }
            }

            double totalMoney =(( money.Sum(m => m.Price * m.Amount) )+ Member.Credit);
            double productPrice = product.Price;
            int creditSub = (int)Member.Credit - (int)productPrice;
            
            Console.WriteLine("eiei" + creditSub);
            await _memberService.SubCredit(Member.Id,creditSub);
            Console.WriteLine("eiei2");
            
            // int totalChange = 0;
            // if (member.Credit == 0)
            // {
            //     totalChange = (int)totalMoney - (int)productPrice;
            // }
            // else
            // {
            //     totalChange = (int)totalMoney - (int)productPrice + (int)member.Credit;
            // }
            int totalChange = (int)totalMoney - (int)productPrice;
            int coinChange = totalChange / 10;
            double remainder = totalChange % 10;
            
            
            
            // int moneyId = moneyData.Id;
            
            if (totalMoney == productPrice)
            {
                 await _productService.SubProduct(producid);
                foreach (var mo in money)
                {   
                    Console.WriteLine("DATA == "+ mo);
                    var moneyData =  await _moneyService.GetMoneyByPriceAsync((double)mo.Price);
                    Console.WriteLine("MoneyID:"+moneyData.Id);
                    int moneyId = moneyData.Id;
                     await _moneyService.AddCoin(moneyId,mo.Amount);
                }
                
                return Ok(new { message = $"Please Take Your Order: {product.Name}" });
            }

           
            var coinInSystem10 =   await _moneyService.GetMoneyByPriceAsync(10);
            var coinInSystem5 =  await _moneyService.GetMoneyByPriceAsync(5);
            var coinInSystem2 =  await _moneyService.GetMoneyByPriceAsync(2);
            var coinInSystem1 =  await _moneyService.GetMoneyByPriceAsync(1);
            string result1 = string.Empty; //*****
            string result2 = null;
            string result3 = null;
            string result4 = null;
            string result5 = null;
            string result6 = null;
            if (totalMoney > productPrice)
            {   
                foreach (var mo in money)
                {   
                  Console.WriteLine("DATA == "+ mo);
                  var moneyData =  await _moneyService.GetMoneyByPriceAsync(mo.Price);
                  int moneyId = moneyData.Id;
                   await _moneyService.AddCoin(moneyId,mo.Amount);
                }
              
                await _productService.SubProduct(producid);
                Console.WriteLine("Money > Price EIEI");
                Console.WriteLine("productPrice " + productPrice);
                Console.WriteLine($"Total money: {totalMoney}");
                Console.WriteLine("totalChange :" +totalChange);
                Console.WriteLine("coinChange :" +coinChange);
                Console.WriteLine("remainder :" + remainder);
                Console.WriteLine("coinInSystem10 :" + coinInSystem10.Amount);
                Console.WriteLine("coinInSystem5 :" + coinInSystem5.Amount);
                Console.WriteLine("coinInSystem2 :" + coinInSystem2.Amount);
                Console.WriteLine("coinInSystem1 :" + coinInSystem1.Amount);
                
                
                if (coinChange > 0)
                {
                    Console.WriteLine("Have Coin change");
                    if (coinChange > 0 && coinInSystem10.Amount != 0)
                    {
                        await _moneyService.SubCoin(coinInSystem10.Id, coinChange);
                        result1 = "Please take your product and change 10 BATH " + coinChange + " coin ";
                    }
                    else if (coinChange > 0 && coinInSystem5.Amount != 0)
                    {
                        int coinChange5 = totalChange / 5 ;
                        await _moneyService.SubCoin(coinInSystem5.Id, coinChange5);
                        result1 = "Please take your product and change 5 BATH " + coinChange5 + " coin ";
                    }
                    else if (coinChange > 0 && coinInSystem2.Amount != 0)
                    {
                        int coinChange2 = totalChange / 2 ;
                        await _moneyService.SubCoin(coinInSystem2.Id, coinChange2);
                        result1 = "Please take your product and change 2 BATH " + coinChange2 + " coin ";
                    }
                    else if (coinChange > 0 && coinInSystem1.Amount != 0)
                    {
                        int coinChange1 = totalChange / 1 ;
                        await _moneyService.SubCoin(coinInSystem1.Id, coinChange1);
                        result1 = "Please take your product and change 1 BATH " + coinChange1 + " coin ";
                    }
                    else
                    {
                      
  
                      await _memberService.AddCredit(tell,coinChange * 10);
                        result1 = "Not have coin change add credit " + coinChange * 10 + " BATH ";
                    }
                }
                
                if (remainder > 0)
                { 
                    Console.WriteLine("Have Remainder");
                    
                  if (remainder >= 5)
                  {
                    switch (remainder)
                    {
                      case >= 5 when coinInSystem5.Amount != 0 && coinInSystem5.Amount >1:
                          Console.WriteLine("HaveFive");
                        await _moneyService.SubCoin(coinInSystem5.Id, 1);
                        result2 =  ", Please take your product and change 5 BATH " +1+ " coin ";
                        break;
                       
                      case >= 5 when coinInSystem2.Amount !=0 && coinInSystem2.Amount > 2 && coinInSystem1.Amount > 1:
                        // coinInSystem[2] -= 2;
                        // coinInSystem[1] -= 1;
                        await _moneyService.SubCoin(coinInSystem2.Id, 2);
                        await _moneyService.SubCoin(coinInSystem1.Id, 1);
                        result2 =  ", Please take your product and change 2 BATH " +2+ " coin and 1 BATH " +1+ " coin ";
                        break;
                        
                      case >= 5 when coinInSystem2.Amount == 1 && coinInSystem2.Amount != 0 && coinInSystem1.Amount > 3:
                        // coinInSystem[2] -= 1;
                        // coinInSystem[1] -= 3;
                        await _moneyService.SubCoin(coinInSystem2.Id, 1);
                        await _moneyService.SubCoin(coinInSystem1.Id, 3);
                        result2 = ", Please take your product and change 2 BATH " +1+ " coin and 1 BATH " +3+ " coin ";
                        break;
                      
                      case >= 5 when coinInSystem1.Amount > 5 && coinInSystem1.Amount != 0:
                        // coinInSystem[1] -= 5;
                        await _moneyService.SubCoin(coinInSystem1.Id, 5);
                        result2 = ", Please take your product and change 1 BATH " +5+ " coin ";
                        break;
                       
                      default:
                        await _memberService.AddCredit(tell, 5);
                        result2 = ", Not have coin change add credit 5 BATH";
                        break;
                      
                    }
                    
                    double remainder2 = remainder % 5;
                    if (remainder2 != 0 && remainder2 == 4)
                    {
                      switch(remainder2)
                      {
                        case 4 when coinInSystem2.Amount != 0 && coinInSystem2.Amount > 1:
                          // coinInSystem[2] -= 2;
                          await _moneyService.SubCoin(coinInSystem2.Id, 2);
                          result3 = ", Please take your product and change 2 BATH " +2+ " coin ";
                          break;
                        case 4 when coinInSystem2.Amount != 0 && coinInSystem2.Amount < 2 && coinInSystem2.Amount != 0 && coinInSystem1.Amount > 1:
                          // coinInSystem[2] -= 1;
                          // coinInSystem[1] -= 2;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          await _moneyService.SubCoin(coinInSystem2.Id, 2);
                          result3 = ", Please take your product and change 2 BATH " +1+ " coin and 1 BATH " +2+ " coin ";
                          break;
                        case 4 when coinInSystem2.Amount == 0 && coinInSystem1.Amount >3 :
                          // coinInSystem[1] -= 4;
                          await _moneyService.SubCoin(coinInSystem1.Id, 4);
                          result3 = ", Please take your product and change 1 BATH " +4+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 4);
                          result3 = ", Not have coin change add credit 4 BATH";
                          break;
                      }

                    }
                    else if(remainder2 != 0 && remainder2 == 3)
                    {
                      switch(remainder2)
                      {
                        case 3 when coinInSystem2.Amount != 0 && coinInSystem2.Amount > 1 && coinInSystem1.Amount > 2 :
                          // coinInSystem[2] -= 1;
                          // coinInSystem[1] -= 1;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          await _moneyService.SubCoin(coinInSystem1.Id, 1);
                          result3 = ", Please take your product and change 2 BATH " +1+ " coin and 1 BATH " +2+ " coin ";
                          break;
                        case 3 when coinInSystem1.Amount > 2 && coinInSystem1.Amount != 0:
                          // coinInSystem[1] -= 3;
                          await _moneyService.SubCoin(coinInSystem1.Id, 3);
                          result3 = ", Please take your product and change 1 BATH " +3+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 3);
                          result3 = ", Not have coin change add credit 3 BATH";
                          break;
                      }
                    }
                    else if(remainder2 != 0 && remainder2 == 2)
                    {
                      switch(remainder2)
                      {
                        case 2 when coinInSystem2.Amount != 0 && coinInSystem2.Amount < 2:
                          // coinInSystem[2] -= 1;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          result3 = ", Please take your product and change 2 BATH " +1+ " coin ";
                          break;
                        case 2 when coinInSystem1.Amount < 3 && coinInSystem1.Amount != 0:
                          // coinInSystem[1] -= 2;
                          await _moneyService.SubCoin(coinInSystem1.Id, 2);
                          result3 = ", Please take your product and change 1 BATH " +2+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 2);
                          result3 = ", Not have coin change add credit 2 BATH";
                          break;
                      }
                    }
                    else if(remainder2 != 0 && remainder2 == 1)
                    {
                      Console.WriteLine("=== 1");
                      switch(remainder2)
                      {
                        
                        case 1 when  coinInSystem1.Amount !=0:
                          //coinInSystem[1] -= 1;
                          await _moneyService.SubCoin(coinInSystem1.Id, 1);
                          result3 = ", Please take your product and change 1 BATH " +1+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 1);
                          result3 = ", Not have coin change add credit 1 BATH";
                          break;
                      }
                    }
                    
                  }
                  
                  
                  
                  
                  else if (remainder < 5)
                  {
                    if (remainder != 0 && remainder == 4)
                    {
                      switch(remainder)
                      {
                        case 4 when coinInSystem2.Amount != 0 && coinInSystem2.Amount > 1:
                          // coinInSystem[2] -= 2; 
                          await _moneyService.SubCoin(coinInSystem2.Id, 2);
                          result4 = ", Please take your product and change 2 BATH " +2+ " coin ";
                          break;
                        case 4 when coinInSystem2.Amount != 0 && coinInSystem2.Amount < 2 && coinInSystem2.Amount != 0 && coinInSystem1.Amount > 1:
                          // coinInSystem[2] -= 1;
                          // coinInSystem[1] -= 2;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          await _moneyService.SubCoin(coinInSystem2.Id, 2);
                          result4 = ", Please take your product and change 2 BATH " +1+ " coin and 1 BATH " +2+ " coin ";
                          break;
                        case 4 when coinInSystem2.Amount == 0 && coinInSystem1.Amount >3 :
                          // coinInSystem[1] -= 4;
                          await _moneyService.SubCoin(coinInSystem1.Id, 4);
                          result4 = ", Please take your product and change 1 BATH " +4+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 4);
                          result4 = ", Not have coin change add credit 4 BATH";
                          break;
                      }

                    }
                    else if(remainder != 0 && remainder == 3)
                    {
                      switch(remainder)
                      {
                        case 3 when coinInSystem2.Amount != 0 && coinInSystem2.Amount > 1 && coinInSystem1.Amount > 2 :
                          // coinInSystem[2] -= 1;
                          // coinInSystem[1] -= 1;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          await _moneyService.SubCoin(coinInSystem1.Id, 1);
                          result4 = ", Please take your product and change 2 BATH " +1+ " coin and 1 BATH " +2+ " coin ";
                          break;
                        case 3 when coinInSystem1.Amount > 2 && coinInSystem1.Amount != 0:
                          // coinInSystem[1] -= 3;
                          await _moneyService.SubCoin(coinInSystem1.Id, 3);
                          result4 = ", Please take your product and change 1 BATH " +3+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 3);
                          result4 = ", Not have coin change add credit 3 BATH";
                          break;
                      }
                    }
                    else if(remainder != 0 && remainder == 2)
                    {
                      switch(remainder)
                      {
                        case 2 when coinInSystem2.Amount != 0 && coinInSystem2.Amount < 2:
                          // coinInSystem[2] -= 1;
                          await _moneyService.SubCoin(coinInSystem2.Id, 1);
                          result4 = ", Please take your product and change 2 BATH " +1+ " coin ";
                          break;
                        case 2 when coinInSystem1.Amount < 3 && coinInSystem1.Amount != 0:
                          // coinInSystem[1] -= 2;
                          await _moneyService.SubCoin(coinInSystem1.Id, 2);
                          result4 = ", Please take your product and change 1 BATH " +2+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 2);
                          result4 = ", Not have coin change add credit 2 BATH";
                          break;
                      }
                    }
                    else if(remainder != 0 && remainder == 1)
                    {
                      switch(remainder)
                      {
                        case 1 when coinInSystem1.Amount !=0:
                          //coinInSystem[1] -= 1;
                          await _moneyService.SubCoin(coinInSystem1.Id, 1);
                          result4 = ", Please take your product and change 1 BATH " +1+ " coin ";
                          break;
                        default:
                          await _memberService.AddCredit(tell, 1);
                          result4 = ", Not have coin change add credit 1 BATH";
                          break;
                      }
                    }
                  }
                  else
                  {
                    result5 = "Error";
                  }
                }
                
                
            }
            else
            {
              result6 = "Error money < productPrice";
            }
            
                string totalResult = $"{result1}{result2}{result3}{result4}{result5}{result6}";

                return Ok(new { message = totalResult });
            



            // return Ok(new { message = "Product sold successfully", totalMoney, money });

          }
          catch (Exception a)
          {
            Console.WriteLine(a);
            throw;
          } 
        }




    }
}