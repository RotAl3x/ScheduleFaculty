import {WeekDay} from "@angular/common";

export interface IClassroom {
  id ?: string,
  name: string|null,
  daysOfWeek: WeekDay[]|null,
}
