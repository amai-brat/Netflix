import 'dart:convert';

class JwtDecoder {
  JwtDecoder._();

  static Map<String, dynamic> decode(String token) {
    final parts = token.split('.');
    if (parts.length != 3) {
      throw Exception('invalid token');
    }

    final payload = _getJsonFromJWT(parts[1]);
    final payloadMap = json.decode(payload);
    if (payloadMap is! Map<String, dynamic>) {
      throw Exception('invalid payload');
    }

    return payloadMap;
  }

  static String _getJsonFromJWT(String encodedStr){
    String normalizedSource = base64Url.normalize(encodedStr);
    return utf8.decode(base64Url.decode(normalizedSource));
  }
}
