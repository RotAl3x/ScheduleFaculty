import {WeekDay} from "@angular/common";

export interface IHourStudyOfAYear {
  id: string,
  semiGroupsId?: string[],
  courseHourTypeId: string,
  userId: string,
  classroomId: string,
  studyWeeks: number[],
  dayOfWeek: WeekDay[],
  startTime: number,
  endTime: number,
}
