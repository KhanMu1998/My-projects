package com.example.servingwebcontent.repos;
import com.example.servingwebcontent.domain.Apart;
import com.example.servingwebcontent.domain.Person;
import org.springframework.data.repository.CrudRepository;

import java.util.List;

public interface ApartRepo extends CrudRepository<Apart, Integer> {
    List<Apart> findByOwner(Person owner);
}
