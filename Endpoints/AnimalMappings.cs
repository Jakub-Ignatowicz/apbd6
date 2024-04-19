using cw6.Validators;
using cw6.Models;
using cw6.Models.DTOs;
using cw6.Repositories;


namespace cw6.Endpoints;

public static class AnimalMappings
{
    public static void MapAnimalEndpoints(this WebApplication app)
    {
        app.MapGet("/animals", (AnimalsRepository db, string orderBy) =>
        {
            List<Animal> animals = db.GetAnimals(orderBy);

            return Results.Ok(animals);
        });

        app.MapGet("/animals/{id}", (int id, AnimalsRepository db) =>
        {
            var animal = db.GetAnimals("").Find(animal => animal.Id == id);

            return animal is not null
                ? Results.Ok(animal)
                : Results.NotFound();
        });

        app.MapPost("/animals", (AddAnimal animal, AnimalsRepository db) =>
        {
            db.AddAnimal(animal);
            return Results.Created("", animal);
        });

        app.MapPut("/animals/{id}", (int id, ChangeAnimal animal, AnimalsRepository db) =>
        {
            var animalToUpdate = db.GetAnimals("").FirstOrDefault(a => a.Id == id);

            if (animalToUpdate == null)
            {
                return Results.NotFound("Cannot find animal with this id");
            }

            db.UpdateAnimal(id, animal);
            return Results.Created($"/animals/{id}", animal);
        });

        app.MapDelete("/animals/{id}", (int id, AnimalsRepository db) =>
        {
            var animalToDelete = db.GetAnimals("").FirstOrDefault(a => a.Id == id);

            if (animalToDelete == null)
            {
                return Results.NotFound("Cannot find animal with this id");
            }

            db.RemoveAnimal(id);
            return Results.NoContent();
        });
    }
}
