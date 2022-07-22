package com.example.servingwebcontent.repos;
import com.example.servingwebcontent.domain.House;
import org.springframework.data.repository.CrudRepository;

public interface HouseRepo extends CrudRepository<House, String>{
}
