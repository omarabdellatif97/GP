import { IApplication } from "./application";
import { ICaseFile } from "./case-file";
import { IStep } from "./step";
import { ITag } from "./tag";

export interface ICase {
    id?: number;
    title: string;
    description: string;
    tags: ITag[];
    steps: IStep[];
    caseFiles: ICaseFile[];
    applications: IApplication[];
}

