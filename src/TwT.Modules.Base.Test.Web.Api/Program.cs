namespace TwT.Modules.Base.Test.Web.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var flasher = new CustomFlasher(args);

			// Add services to the container.
			flasher.ApplicationBuilder.Services.AddControllers();
			flasher.ApplicationBuilder.Services.AddEndpointsApiExplorer();
			flasher.ApplicationBuilder.Services.AddSwaggerGen();

			//Boot the 'flashed' logic
			flasher.BootUp();

			// Configure the HTTP request pipeline.
			if (flasher.Application.Environment.IsDevelopment())
			{
				flasher.Application.UseSwagger();
				flasher.Application.UseSwaggerUI();
			}

			flasher.Application.UseHttpsRedirection();
			flasher.Application.UseAuthorization();
			flasher.Application.MapControllers();
			flasher.Application.Run();
		}
	}
}