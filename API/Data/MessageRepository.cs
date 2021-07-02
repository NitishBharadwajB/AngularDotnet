using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                            .Include(x => x.Sender)
                            .Include(x => x.Recipient)
                            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.UserName && u.RecipientDeleted == false),
                "Outbox" => query.Where(o => o.Sender.UserName == messageParams.UserName && o.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.UserName && u.DateRead == null && u.RecipientDeleted == false)
            };

            // if(messageParams.Container == "Inbox")
            // {
            //     var unreadMessages = query.Where(u => u.DateRead == null && u.Recipient.UserName == messageParams.UserName).ToList();

            // if(unreadMessages.Any())
            // {
            //     foreach(var message in unreadMessages)
            //     {
            //         message.DateRead = DateTime.Now;
            //     }

            //     await _context.SaveChangesAsync();
            // }
            // }

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages,messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientname)
        {
            var messages = await _context.Messages
            .Include(m => m.Sender).ThenInclude(p => p.Photos)
            .Include(m => m.Recipient).ThenInclude(p => p.Photos)
            .Where(
                m => m.Sender.UserName == recipientname && m.RecipientDeleted ==false &&
                           m.Recipient.UserName == currentUsername ||
                           m.Sender.UserName == currentUsername && m.SenderDeleted == false &&
                           m.Recipient.UserName == recipientname
            ).OrderBy(m => m.MessageSent).ToListAsync();

            var unreadMessages = messages.Where(u => u.DateRead == null && u.Recipient.UserName == currentUsername).ToList();

            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}