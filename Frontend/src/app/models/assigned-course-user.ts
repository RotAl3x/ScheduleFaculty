import {ICourse} from "./course";

export interface IAssignedCourseUser {
  id: string | null,
  professorUserId: string | null,
  courseId: string | null,
}

export interface IAssignedCourseUserResponse {
  id: string | null,
  professorUserId: string | null,
  course: ICourse | null,
}
