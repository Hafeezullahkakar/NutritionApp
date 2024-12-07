import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FoodService {
 
  private foodApiURL = 'https://localhost:7060/api/FoodItem'; // Backend URL

  constructor(private http: HttpClient) {}

  getAllFoodItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.foodApiURL}/get-all-item`);
  }
}
