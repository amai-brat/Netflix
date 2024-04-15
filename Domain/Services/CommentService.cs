using Domain.Abstractions;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
	public class CommentService(ICommentRepository commentRepository): ICommentService
	{
		public async Task<Comment> DeleteCommentByIdAsync(long id)
		{
			var comment = await commentRepository.GetCommentByIdAsync(id);

			if (comment == null) 
			{
				throw new CommentServiceArgumentException(ErrorMessages.NotFoundComment, nameof(id));
			}

			comment = commentRepository.Remove(comment);

			await commentRepository.SaveChangesAsync();
			return comment;
		}
	}
}
