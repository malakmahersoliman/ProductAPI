using MediatR;

namespace ProductAPI.Feature.Categories.Command.DeleteCategory
{

    public class DeleteCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }

    }
}
