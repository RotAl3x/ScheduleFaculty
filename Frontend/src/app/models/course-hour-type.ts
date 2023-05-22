import {IHourType} from "./hour-type";
import {ICourse} from "./course";

export interface ICourseHourType {
  id?: string,
  courseId: string,
  hourType: IHourType,
}

export interface ICourseHourTypeResponse {
  id?: string,
  course: ICourse,
  hourType: IHourType,
}
