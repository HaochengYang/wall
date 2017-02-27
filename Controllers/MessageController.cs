
using wall.Models;
using wall.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace wall.Controllers{
    public class Message : Controller{
        private readonly MessageFactory _messageFactory;

        private readonly UserFactory _userFactory;

        public Message(MessageFactory messageFactory, UserFactory userFactory){
           _messageFactory = messageFactory;
           _userFactory = userFactory;
        }


        [HttpGet]
        [Route("dashbord")]
        public IActionResult dashbord(){
            if(!CheckLogin())
            {
                return RedirectToAction("RegisterPage", "Users");
            }
            List<Message> AllMessages = _messageFactory.GetAllMessages();
            ViewBag.CurrentUser = _userFactory.GetUserById((int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.Messages = AllMessages;
            return View();
        }

        [HttpPost]
        [Route("createMessage")]
        public IActionResult createMessage(Message newMessage){
            if(!CheckLogin())
            {
                return RedirectToAction("RegisterPage", "Users");
            }
            if(ModelState.IsValid){
              newMessage.postid = (int)HttpContext.Session.GetInt32("CurrUserid");
              _messageFactory.Add(newMessage);
            }
            List<Message>AllMessages = _messageFactory.GetAllMessages();
            ViewBag.CurrentUser = _userFactory.GetUserById((int)HttpContext.Session.GetInt32("CurrUserid"));
            ViewBag.Messages = AllMessages;
            return View("dashbord");
        }

        [HttpPost]
        [Route("DeleteMessage/{messageid}")]
        public IActionResult DeleteMessage(int messageid){
          if(!CheckLogin())
            {
                return RedirectToAction("RegisterPage", "Users");
            }
          Message.CheckMessage = _messageFactory.GetMessageById(messageid);
          if(CheckMessage.posterid == (int)HttpContext.Session.GetInt32("CurrUserid")&& CheckMessage.Deletable){
              -messageFactory.DeleteMessage(messageid);
          }
          return RedirectToAction("dashbord");
        }
        private bool CheckLogin(){
            return (HttpContext.Session.GetInt32("CurrUserid") != null);
        }
    }
}