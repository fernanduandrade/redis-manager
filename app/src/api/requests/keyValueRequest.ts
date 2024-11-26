import { Connection } from "../../common/domain/connection";

export interface keyValueRequest {
    id: string
    key: string
    connection: Connection
}