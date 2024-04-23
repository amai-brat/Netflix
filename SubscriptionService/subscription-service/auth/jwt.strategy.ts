import { ExtractJwt, Strategy } from 'passport-jwt';
import { PassportStrategy } from '@nestjs/passport';
import { Injectable } from '@nestjs/common';

@Injectable()
export class JwtStrategy extends PassportStrategy(Strategy) {
    constructor() {
        super({
            jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
            secretOrKey: "000011112222333344445555666677778888" //TODO брать ключ из переменной среды, тот же что и в ASP.net
        });
    }

    async validate(payload: any) {
        return { id: payload.id, role: payload.role };
    }
}