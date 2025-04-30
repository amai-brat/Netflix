enum UserStatus { initial, loading, authenticated, unauthenticated }

class UserState {
  final UserStatus status;
  final bool isAuthenticated;
  final int? userId;
  final String error;

  UserState({
    required this.status,
    required this.isAuthenticated,
    this.userId,
    required this.error,
  });

  UserState.initial()
    : status = UserStatus.initial,
      isAuthenticated = false,
      userId = null,
      error = '';

  UserState copyWith({
    UserStatus? status,
    bool? isAuthenticated,
    int? userId,
    String? error,
  }) {
    return UserState(
      status: status ?? this.status,
      isAuthenticated: isAuthenticated ?? this.isAuthenticated,
      userId: userId ?? this.userId,
      error: error ?? this.error,
    );
  }
}
