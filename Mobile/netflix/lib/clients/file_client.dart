import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:fixnum/fixnum.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;
import 'package:mime/mime.dart';
import 'package:mime_type/mime_type.dart';
import 'package:file_picker/file_picker.dart';
import 'package:http_parser/http_parser.dart';
import 'package:netflix/utils/consts.dart';
import 'package:netflix/utils/di.dart';
import 'package:netflix/utils/jwt_decoder.dart';
import 'package:path_provider/path_provider.dart';
import 'package:uuid/uuid.dart';

class FileClient {
  final Map<String, CachedFile> fileCache = {};
  final String baseUrl;
  final String downloadHost;
  final http.Client _client;

  FileClient({
    required this.baseUrl,
    required this.downloadHost,
    http.Client? client,
  }) : _client = client ?? http.Client();

  Future<List<String>> uploadFiles({
    required List<PlatformFile> files
  }) async {
    try {
      var accessToken = await locator<FlutterSecureStorage>().read(key: Consts.accessToken);
      final userId = Int64.parseInt(JwtDecoder.decode(accessToken!)['id']);

      final uri = Uri.parse('$baseUrl/support/chats/$userId/files/upload');
      final request = http.MultipartRequest('POST', uri);

      request.headers['Authorization'] = 'Bearer $accessToken';

      for (final file in files) {
        final mimeType = _getMimeType(file.name);
        final mediaType = MediaType.parse(mimeType);

        if (file.path != null) {
          final fileStream = File(file.path!).openRead();
          final length = await File(file.path!).length();

          request.files.add(http.MultipartFile(
            'files',
            fileStream,
            length,
            filename: file.name,
            contentType: mediaType,
          ));
        } else if (file.bytes != null) {
          request.files.add(http.MultipartFile.fromBytes(
            'files',
            file.bytes!,
            filename: file.name,
            contentType: mediaType,
          ));
        } else {
          throw Exception('Файл ${file.name} пустой');
        }
      }

      final response = await _client.send(request);
      return await _handleResponse(response);
    } catch (e) {
      throw Exception('Ошибка загрузки: ${e.toString()}');
    }
  }

  String _getMimeType(String fileName) {
    return mime(fileName) ?? 'application/octet-stream';
  }


  Future<List<String>> _handleResponse(http.StreamedResponse response) async {
    final responseBody = await response.stream.bytesToString();
    final statusCode = response.statusCode;

    if (statusCode >= 200 && statusCode < 300) {
      final jsonResponse = jsonDecode(responseBody) as List<dynamic>;
      return jsonResponse.map((url) => url.toString()).toList();
    } else {
      final errorMessage = 'Загрузка закончилась со статусом $statusCode';
      throw Exception(errorMessage);
    }
  }

  Future<PlatformFile?> downloadFile(String url) async {
    try {
      if (fileCache.containsKey(url)) {
        final cachedFile = fileCache[url]!;
        return PlatformFile(
          name: cachedFile.name,
          path: cachedFile.path,
          size: cachedFile.size,
        );
      }

      final response = await http.get(Uri.parse(url.replaceFirst('localhost', downloadHost)));

      if (response.statusCode == 200) {
        final directory = await getTemporaryDirectory();

        final mimeType = lookupMimeType('', headerBytes: response.bodyBytes);
        var extension = mimeType == null ? 'bin': mimeType.split('/').last;
        final fileName = '${Uuid().v4()}.$extension';

        final filePath = '${directory.path}/$fileName';
        final file = File(filePath);

        await file.writeAsBytes(response.bodyBytes);

        final fileLength =  await file.length();

        fileCache[url] = CachedFile(
          url: url,
          name: fileName,
          path: file.path,
          size: fileLength,
        );

        return PlatformFile(
          name: fileName,
          path: file.path,
          size: fileLength,
        );
      } else {
        return null;
      }
    } catch (e) {
      return null;
    }
  }

  void close() {
    _client.close();
  }
}

class CachedFile {
  final String url;
  final String name;
  final String path;
  final int size;

  CachedFile({
    required this.url,
    required this.name,
    required this.path,
    required this.size,
  });
}