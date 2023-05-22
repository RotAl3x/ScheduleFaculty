import {WeekDay} from "@angular/common";
import {IClassroom} from "./classroom";
import {IUser} from "./login";
import {ICourseHourTypeResponse} from "./course-hour-type";
import {IStudyYearGroup} from "./study-year-group";

export interface IHourStudyOfAYear {
  id: string | null,
  semiGroupsId: string[] | null,
  courseHourTypeId: string | null,
  userId: string | null,
  classroomId: string | null,
  studyWeeks: number[] | null,
  dayOfWeek: WeekDay | null,
  startTime: number | null,
  endTime: number | null,
}

export interface IHourStudyOfAYearResponse {
  id: string,
  semiGroups: IStudyYearGroup[],
  courseHourType: ICourseHourTypeResponse,
  user: IUser,
  classroom: IClassroom,
  studyWeeks: number[],
  dayOfWeek: WeekDay,
  startTime: number,
  endTime: number,
}
