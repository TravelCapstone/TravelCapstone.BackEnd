using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Enum
{
    public enum DietaryPreference
    {
        OMNIVORES,// ăn hết, không tôn giáo
        VEGAN,// Thuần chay
        VEGETARIAN,// Chay trường
        GLUTEN_FREE,//Không chứa protein từ gạo và nũ cốc
        HALAL,//Đạo Hồi
        KOSHER,//Đạo Do Thái
        PESCATARIAN//Chay nhưng ăn cá
    }
}
