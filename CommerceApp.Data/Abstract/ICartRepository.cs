﻿using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Abstract
{
    public interface ICartRepository:IRepository<Cart>
    {
        Cart getCartByUserId(string userId);
        void AddToCart(string userid, int productid, int quantity);
        void DeleteFromCart(int cartId, int productId);
    }
}