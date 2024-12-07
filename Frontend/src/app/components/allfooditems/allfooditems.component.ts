import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from "../shared/navbar/navbar.component";
import { identifierName } from '@angular/compiler';

@Component({
  selector: 'app-allfooditems',
  templateUrl: './allfooditems.component.html',
  styleUrls: ['./allfooditems.component.scss'],
  standalone: true,
  imports: [CommonModule, NavbarComponent]
})
export class AllFoodItemsComponent implements OnInit {
  foodItems: any[] = [];
  loading: boolean = true;
  errorMessage: string | null = null;

  constructor(private foodService: FoodService) {}

  ngOnInit(): void {
    this.fetchFoodItems();
  }

  fetchFoodItems(): void {
    this.foodService.getAllFoodItems().subscribe({
      next: (data) => {
        console.log('Data: ', data)
        // Populate the food items array with necessary fields
        this.foodItems = data.map((item) => ({
          name: item.name,
          type: item.type,
          identifier: item.identifier,
          amount: item.type === 'Countable' ? item.count : item.weight,
          fats: item.nutrient?.fats || 0,
          protein: item.nutrient?.protein || 0,
          carbs: item.nutrient?.carbs || 0,
        }));
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load food items.';
        this.loading = false;
        console.error(error);
      },
    });
  }
}
