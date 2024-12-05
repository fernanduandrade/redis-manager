import { RedisKey } from "./redisKey"

export type Connection = {
    host: string
    port: string
    password: string
    username: string
    name?: string
    open?: boolean
    id?: any
    keyspaces?: RedisKey[]
}