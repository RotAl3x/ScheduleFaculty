import {IStudyProgram} from "./study-program";
import {IUser} from "./login";
import {IHourType} from "./hour-type";

export interface ICourse {
  id?: string | null,
  studyProgramYearId: string | null,
  professorUserId: string | null,
  name: string | null,
  abbreviation: string | null,
  semester: number | null,
  isOptional: boolean | null
}

export interface ICourseResponse {
  id: string,
  studyProgram: IStudyProgram,
  professorUser: IUser,
  hourTypes: IHourType[],
  name: string | null,
  abbreviation: string | null,
  semester: number | null,
  isOptional: boolean | null
}



