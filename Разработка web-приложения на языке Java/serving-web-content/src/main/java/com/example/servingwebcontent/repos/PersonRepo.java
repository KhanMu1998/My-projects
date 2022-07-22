package com.example.servingwebcontent.repos;
import com.example.servingwebcontent.domain.Person;
import org.springframework.data.repository.CrudRepository;

public interface PersonRepo extends CrudRepository<Person, String> {
}
