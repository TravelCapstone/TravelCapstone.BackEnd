﻿using System.Globalization;

namespace TravelCapstone.BackEnd.Common.Utils;

public class SD
{
    public static int MAX_RECORD_PER_PAGE = short.MaxValue;
    public static string DEFAULT_PASSWORD = "TourGuide@123";
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy");
    }
    public static string GenerateOTP(int length = 6)
    {
        // Sử dụng thư viện Random để tạo mã ngẫu nhiên
        Random random = new Random();

        // Tạo chuỗi ngẫu nhiên với độ dài mong muốn
        string otp = "";
        for (int i = 0; i < length; i++)
        {
            otp += random.Next(0, 10); // Sinh số ngẫu nhiên từ 0 đến 9 và thêm vào chuỗi OTP
        }

        return otp;
    }
    public static IEnumerable<WeekForYear> PrintWeeksForYear(int year)
    {
        var weekForYears = new List<WeekForYear>();
        var startDate = new DateTime(year, 1, 1);
        var endDate = startDate.AddDays(6);

        var cultureInfo = CultureInfo.CurrentCulture;

        Console.WriteLine($"Week 1: {startDate.ToString("d", cultureInfo)} - {endDate.ToString("d", cultureInfo)}");
        weekForYears.Add(new WeekForYear
        {
            WeekIndex = 1,
            Timeline = new WeekForYear.TimelineDto
            { StartDate = startDate.ToString("d", cultureInfo), EndDate = endDate.ToString("d", cultureInfo) }
        });

        for (var week = 2; week < 53; week++)
        {
            startDate = endDate.AddDays(1);
            endDate = startDate.AddDays(6);

            Console.WriteLine(
                $"Week {week}: {startDate.ToString("d", cultureInfo)} - {endDate.ToString("d", cultureInfo)}");
            weekForYears.Add(new WeekForYear
            {
                WeekIndex = week,
                Timeline = new WeekForYear.TimelineDto
                { StartDate = startDate.ToString("d", cultureInfo), EndDate = endDate.ToString("d", cultureInfo) }
            });
        }

        return weekForYears;
    }

    public class ResponseMessage
    {
        public static string CREATE_SUCCESSFULLY = "Tạo mới thành công";
        public static string UPDATE_SUCCESSFULLY = "Cập nhật thành công";
        public static string DELETE_SUCCESSFULY = "Xóa thành công";
        public static string CREATE_FAILED = "Có lỗi xảy ra trong quá trình tạo";
        public static string UPDATE_FAILED = "Có lỗi xảy ra trong quá trình cập nhật";
        public static string DELETE_FAILED = "Có lỗi xảy ra trong quá trình xóa";
        public static string LOGIN_FAILED = "Đăng nhập thất bại";
    }

    public class SubjectMail
    {
        public static string VERIFY_ACCOUNT = "[TRAVEL]  CHÀO MỪNG BẠN ĐẾN VỚI TRAVEL. VUI LÒNG XÁC MINH TÀI KHOẢN";
        public static string WELCOME = "[TRAVEL] CHÀO MỪNG BẠN ĐẾN VỚI TRAVEL";
        public static string REMIND_PAYMENT = "[TRAVEL] NHẮC NHỞ THANH TOÁN";
        public static string PASSCODE_FORGOT_PASSWORD = "[TRAVEL] MÃ XÁC THỰC QUÊN MẬT KHẨU";

        public static string SIGN_CONTRACT_VERIFICATION_CODE =
            "[LOVE HOUSE] You are in the process of completing contract procedures".ToUpper();
    }

    public class WeekForYear
    {
        public int WeekIndex { get; set; }
        public TimelineDto? Timeline { get; set; }

        public class TimelineDto
        {
            public string? StartDate { get; set; }
            public string? EndDate { get; set; }
        }
    }

    public class Regex
    {
        public static string PHONENUMBER = "^0\\d{9,10}$";
        public static string EMAIL = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        public static string NAME = "^[\\p{L}\\s,'.-]+$";
    }
    public class EnumType
    {
        public static Dictionary<string, int> MaterialUnit = new Dictionary<string, int> { { "KG", 0 }, { "M3", 1 }, { "BAR", 2 }, { "ITEM", 3 }, { "Kg", 0 }, { "m3", 1 }, { "Bar", 2 }, { "Item", 3 }, { "kg", 0 }, { "bar", 2 }, { "item", 3 } };
        public static Dictionary<string, int> ServiceCostUnit = new Dictionary<string, int> { { "DAY", 0 }, { "Day", 0 }, { "day", 0 }, { "ROOM", 1 }, { "Room", 1 }, { "room", 1 }, { "PERSON", 2 }, { "Person", 2 }, { "person", 2 } };
        public static Dictionary<string, int> VehicleType = new Dictionary<string, int> { { "DAY", 0 }, { "Day", 0 }, { "day", 0 }, { "ROOM", 1 }, { "Room", 1 }, { "room", 1 }, { "PERSON", 2 }, { "Person", 2 }, { "person", 2 } };

    }

    public class ExcelHeaders
    {
        public static List<String> SERVICE_QUOTATION = new List<string> { "No", "Tên dịch vụ", "Đơn vị", "MOQ", "Giá người lớn", "Giá trẻ em" };
        public static List<String> MENU_SERVICE_QUOTATION = new List<string> { "No", "Tên dịch vụ", "Đơn vị", "MOQ", "Giá người lớn", "Giá trẻ em" };
        public static List<String> TOURGUIDE_REGISTRATION = new List<string> { "No", "Tên", "Họ", "Email", "Số điện thoại", "Giới tính" };
        public static List<String> VEHICLE_REGISTRATION = new List<string> { "No", "Biển số", "Dung tích", "Số động cơ", "Số khung", "Thương hiệu", "Tên chủ sở hữu", "Màu" };

    }


}
