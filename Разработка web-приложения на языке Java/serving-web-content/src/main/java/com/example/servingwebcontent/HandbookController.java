package com.example.servingwebcontent;

import com.example.servingwebcontent.domain.Apart;
import com.example.servingwebcontent.domain.House;
import com.example.servingwebcontent.domain.Person;
import com.example.servingwebcontent.repos.HouseRepo;
import com.example.servingwebcontent.repos.PersonRepo;
import com.example.servingwebcontent.repos.ApartRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import java.util.Map;


@Controller
public class HandbookController {
    @Autowired
    private PersonRepo personRepo;
    @Autowired
    private ApartRepo apartRepo;
    @Autowired
    private HouseRepo houseRepo;

    @GetMapping
    public String main(Map<String, Object> model) {
        return "main";
    }

    @GetMapping("/persons")
    public String readPersons(Map<String, Object> model) {
        Iterable<Person> persons = personRepo.findAll();
        model.put("persons", persons);
        String error = "Введите ФИО";
        model.put("error", error);
        return "persons";
    }
    @PostMapping("addPerson")
    public String addPerson(@RequestParam String fio, @RequestParam String passport,
                            @RequestParam String phone, Map<String, Object> model) {
        Person person = new Person(fio, passport, phone);
        personRepo.save(person);
        Iterable<Person> persons = personRepo.findAll();
        model.put("persons", persons);
        String error = "Введите ФИО";
        model.put("error", error);
        return "persons";
    }
    @PostMapping("editPerson")
    public String editPerson(@RequestParam String fio, @RequestParam String passport,
                              @RequestParam String phone, Map<String, Object> model) {
        Person person = personRepo.findById(fio).orElseThrow();
        person.setPassport(passport);
        person.setPhone(phone);
        personRepo.save(person);
        Iterable<Person> persons = personRepo.findAll();
        model.put("persons", persons);
        String error = "Введите ФИО";
        model.put("error", error);
        return "persons";
    }
    @PostMapping("editPersonSelect")
    public String editPersonSelect(@RequestParam String fio, Map<String, Object> model) {
        Person person = personRepo.findById(fio).orElseThrow();
        model.put("persons", person);
        return "personsSelect";
    }
    @PostMapping("deletePerson")
    public String deletePerson(@RequestParam String fio, Map<String, Object> model) {
        Person person = personRepo.findById(fio).orElseThrow();
        Iterable<Apart> aparts = apartRepo.findByOwner(person);
        String error = "Введите ФИО";
        int counter = 0;
        for (Object i : aparts) {
            counter++;
        }
        if (counter == 0) {
            personRepo.delete(person);
        }
        else {
            error = "Нельзя удалить владельца квартиры!";

        }
        model.put("error", error);
        Iterable<Person> persons = personRepo.findAll();
        model.put("persons", persons);
        return "persons";
    }

    @GetMapping("/houses")
    public String readHouses(Map<String, Object> model) {
        Iterable<House> houses = houseRepo.findAll();
        model.put("houses", houses);
        return "houses";
    }

    @PostMapping("addHouse")
    public String addHouse(@RequestParam String address, @RequestParam Integer floors,
                            @RequestParam Integer entrances, @RequestParam Integer aparts,
                           Map<String, Object> model) {
        House house = new House(address, floors, entrances, aparts);
        houseRepo.save(house);
        Iterable<House> houses = houseRepo.findAll();
        model.put("houses", houses);
        return "houses";
    }

    @PostMapping("editHouse")
    public String editHouse(@RequestParam String address, @RequestParam Integer floors,
                            @RequestParam Integer entrances, @RequestParam Integer aparts,
                            Map<String, Object> model) {
        House house = houseRepo.findById(address).orElseThrow();
        house.setFloorsNumber(floors);
        house.setEntrancesNumber(entrances);
        house.setApartsNumber(aparts);
        houseRepo.save(house);
        Iterable<House> houses = houseRepo.findAll();
        model.put("houses", houses);
        return "houses";
    }
    @PostMapping("deleteHouse")
    public String deleteHouse(@RequestParam String address, Map<String, Object> model) {
        House house = houseRepo.findById(address).orElseThrow();
        houseRepo.delete(house);
        Iterable<House> houses = houseRepo.findAll();
        model.put("houses", houses);
        return "houses";
    }

    @GetMapping("/aparts")
    public String readApart(Map<String, Object> model) {
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "aparts";
    }

    @PostMapping("addApart")
    public String addApart(@RequestParam Integer apartNumber, @RequestParam Integer floor,
                           @RequestParam Integer entrance, @RequestParam String owner,
                           @RequestParam String address, Map<String, Object> model) {
        Person person = personRepo.findById(owner).orElseThrow();
        House house = houseRepo.findById(address).orElseThrow();
        Apart apart = new Apart(apartNumber, floor, entrance, person, house);
        apartRepo.save(apart);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "aparts";
    }

    @PostMapping("editApart")
    public String editApart(@RequestParam Integer apartNumber, @RequestParam Integer floor,
                            @RequestParam Integer entrance, @RequestParam String owner,
                            @RequestParam String address, @RequestParam Integer apartId,
                            Map<String, Object> model) {
        Apart apart = apartRepo.findById(apartId).orElseThrow();
        Person person = personRepo.findById(owner).orElseThrow();
        House house = houseRepo.findById(address).orElseThrow();
        apart.setApartNumber(apartNumber);
        apart.setFloor(floor);
        apart.setEntrance(entrance);
        apart.setOwner(person);
        apart.setAddress(house);
        apartRepo.save(apart);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "aparts";
    }
    @PostMapping("deleteApart")
    public String deleteApart(@RequestParam Integer apartId, Map<String, Object> model) {
        Apart apart = apartRepo.findById(apartId).orElseThrow();
        apartRepo.delete(apart);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "aparts";
    }

    @GetMapping("/handbook")
    public String mainH(Map<String, Object> model) {
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "handbook";
    }

    @PostMapping("/addPersonH")
    public String addPersonH(@RequestParam String fio, @RequestParam String passport,
                      @RequestParam String phone, Map<String, Object> model) {
        Person person = new Person(fio, passport, phone);
        personRepo.save(person);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "handbook";
    }
    @PostMapping("editPersonH")
    public String editPersonH(@RequestParam String fio, @RequestParam String passport,
                            @RequestParam String phone, Map<String, Object> model) {
        Person person = personRepo.findById(fio).orElseThrow();
        person.setPassport(passport);
        person.setPhone(phone);
        personRepo.save(person);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "handbook";
    }
    @PostMapping("deletePersonH")
    public String deletePersonH(@RequestParam String fio, Map<String, Object> model) {
        Person person = personRepo.findById(fio).orElseThrow();
        personRepo.delete(person);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "handbook";
    }
    @PostMapping("apart")
    public String addApartH(@RequestParam Integer apartNumber, @RequestParam Integer floor,
                      @RequestParam Integer entrance, @RequestParam String owner,
                      @RequestParam String address, Map<String, Object> model) {
        Person person = personRepo.findById(owner).orElseThrow();
        House house = houseRepo.findById(address).orElseThrow();
        Apart apart = new Apart(apartNumber, floor, entrance, person, house);
        apartRepo.save(apart);
        Iterable<Person> persons = personRepo.findAll();
        Iterable<Apart> aparts = apartRepo.findAll();
        Iterable<House> houses = houseRepo.findAll();
        model.put("persons", persons);
        model.put("aparts", aparts);
        model.put("houses", houses);
        return "handbook";
    }
}
