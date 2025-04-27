enum UserStatus { initial, loading, authenticated, unauthenticated }

class UserState {
  final UserStatus status;
  final bool isAuthenticated;
  final String? error;

  UserState({required this.status, required this.isAuthenticated, this.error});

  UserState.initial() : status = UserStatus.initial, isAuthenticated = false, error = null;

  UserState copyWith({UserStatus? status, bool? isAuthenticated, String? error}) {
    return UserState(
      status: status ?? this.status,
      isAuthenticated: isAuthenticated ?? this.isAuthenticated,
      error: error ?? this.error
    );
  }
}
