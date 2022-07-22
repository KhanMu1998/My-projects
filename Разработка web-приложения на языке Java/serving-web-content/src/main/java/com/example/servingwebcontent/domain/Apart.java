package com.example.servingwebcontent.domain;
import javax.persistence.*;

@Entity
public class Apart {
    @Id
    @GeneratedValue(strategy=GenerationType.AUTO)
    private Integer apartId;
    private Integer apartNumber;
    private Integer floor;
    private Integer entrance;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "person.fio")
    private Person owner;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "house.address")
    private House address;

    public Apart() {

    }

    public Apart(Integer apartNumber, Integer floor, Integer entrance, Person owner, House address) {
        this.apartNumber = apartNumber;
        this.floor = floor;
        this.entrance = entrance;
        this.owner = owner;
        this.address = address;
    }

    public Integer getApartId() {
        return apartId;
    }

    public void setApartId(Integer apartId) {
        this.apartId = apartId;
    }

    public Integer getApartNumber() {
        return apartNumber;
    }

    public void setApartNumber(Integer apartNumber) {
        this.apartNumber = apartNumber;
    }

    public Integer getFloor() {
        return floor;
    }

    public void setFloor(Integer floor) {
        this.floor = floor;
    }

    public Integer getEntrance() {
        return entrance;
    }

    public void setEntrance(Integer entrance) {
        this.entrance = entrance;
    }

    public Person getOwner() {
        return owner;
    }

    public void setOwner(Person owner) {
        this.owner = owner;
    }

    public House getAddress() {
        return address;
    }

    public void setAddress(House address) {
        this.address = address;
    }
}
