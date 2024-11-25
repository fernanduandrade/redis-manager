export type RedisKey = {
    id: string
    type: KeyType
    name: string
    expanded: boolean
    children?: Array<RedisKey>
}

export type KeyType = 'keyspace' | 'key'