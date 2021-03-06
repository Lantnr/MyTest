IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_user_keep]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_user_keep]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_record_day]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_record_day]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_record_day]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [sp_record_day]
AS
BEGIN
	DECLARE @_online INT,@_offline INT,@_register INT,@_register_total INT,@_taday_online INT,@_history_online INT,@_taday_login INT;
	DECLARE @_begin_time BIGINT,@_taday_time BIGINT,@_next_time BIGINT,@_end_time BIGINT;
	SET @_taday_time=CAST(DATEDIFF(s,''1970-01-01 00:00:00'',GETDATE()) AS BIGINT) * 10000000 + 621355968000000000 
	SET @_next_time=CAST(DATEDIFF(s,''1970-01-01 00:00:00'',GETDATE()+1) AS BIGINT) * 10000000 + 621355968000000000 
	SET @_begin_time=(CAST(DATEDIFF(d,''1970-01-01 00:00:00'',DATEADD(s,(@_taday_time-621355968000000000)/ 10000000,''1970-01-01 00:00:00''))AS BIGINT) * 10000000*24*60*60 + 621355968000000000); 
	SET @_end_time=(CAST(DATEDIFF(d,''1970-01-01 00:00:00'',DATEADD(s,(@_next_time-621355968000000000)/ 10000000,''1970-01-01 00:00:00''))AS BIGINT) * 10000000*24*60*60 + 621355968000000000); 		
	DECLARE @C INT;
	SELECT @C=COUNT(1) FROM [report_day] WHERE createtime>@_begin_time and createtime<@_end_time
	IF @C=0
	BEGIN
		SELECT @_online=COUNT(0) FROM tg_user_login_log WHERE login_state=1
		SELECT @_offline=COUNT(0) FROM tg_user_login_log WHERE login_state=0
		SELECT @_register=COUNT(0) FROM view_player where  createtime>@_begin_time and createtime<@_end_time
		SELECT @_register_total=COUNT(0) FROM view_player
		SET @_taday_online=@_online
		SELECT @_taday_login=COUNT(0) FROM [report_record_login] where  createtime>@_begin_time and createtime<@_end_time
		DECLARE @count INT;
		SELECT @count=COUNT(*) FROM [report_day]
		IF(@count=0)
			BEGIN
				SET @_history_online=0;
			END
		ELSE
			BEGIN
				SELECT TOP 1 @_history_online =[history_online] FROM [report_day] ORDER BY [createtime] DESC
			END
		INSERT INTO [report_day]([online],[offline],[register],[register_total],[taday_online],[history_online],[taday_login],[createtime])
			 VALUES(@_online,@_offline,@_register,@_register_total,@_taday_online,@_history_online,@_taday_login,@_taday_time)
	END
END
' 
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_user_keep]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [sp_user_keep]
	 @open_time BIGINT
AS
BEGIN
	DECLARE @time_0_0 BIGINT,@time_1_0 BIGINT;
	--1天:864000000000
	--@time_0:开服当天0点时间
	--@time_1:开服第二天0点时间
	SET @time_0_0=(CAST(DATEDIFF(d,''1970-01-01 00:00:00'',DATEADD(s,(@open_time-621355968000000000)/ 10000000,''1970-01-01 00:00:00''))AS BIGINT) * 10000000*24*60*60 + 621355968000000000); 
	SET @time_1_0=@time_0_0+864000000000
	--次日留存
	DECLARE @time_2_0 BIGINT,@D_1 INT,@R_1 INT,@N_1 INT; 
	SET @time_2_0=@time_0_0+(864000000000*2)
	SELECT @D_1=COUNT(*) FROM (SELECT DISTINCT [user_id] FROM [report_record_login] WHERE [createtime]>@time_1_0 AND [createtime]<@time_2_0) AS T
	SELECT @N_1=COUNT(*) FROM [tg_user] WHERE [createtime]>@time_1_0 AND [createtime]<@time_2_0
	SELECT @R_1=COUNT(*) FROM [tg_user] WHERE [createtime]<@time_1_0
	--3日留存
	DECLARE @time_3_0 BIGINT,@time_4_0 BIGINT,@D_3 INT,@R_3 INT,@N_3 INT; 
	SET @time_3_0=@time_0_0+(864000000000*3)
	SET @time_4_0=@time_0_0+(864000000000*4)
	SELECT @D_3=COUNT(*) FROM (SELECT DISTINCT [user_id] FROM [report_record_login] WHERE [createtime]>@time_1_0 AND [createtime]<@time_4_0) AS T
	SELECT @N_3=COUNT(*) FROM [tg_user] WHERE [createtime]>@time_3_0 AND [createtime]<@time_4_0
	SELECT @R_3=COUNT(*) FROM [tg_user] WHERE [createtime]<@time_3_0
	--5日留存
	DECLARE @time_5_0 BIGINT,@time_6_0 BIGINT,@D_5 INT,@R_5 INT,@N_5 INT; 
	SET @time_5_0=@time_0_0+(864000000000*5)
	SET @time_6_0=@time_0_0+(864000000000*6)
	SELECT @D_5=COUNT(*) FROM (SELECT DISTINCT [user_id] FROM [report_record_login] WHERE [createtime]>@time_1_0 AND [createtime]<@time_6_0) AS T
	SELECT @N_5=COUNT(*) FROM [tg_user] WHERE [createtime]>@time_5_0 AND [createtime]<@time_6_0
	SELECT @R_5=COUNT(*) FROM [tg_user] WHERE [createtime]<@time_5_0
	--7日留存
	DECLARE @time_7_0 BIGINT,@time_8_0 BIGINT,@D_7 INT,@R_7 INT,@N_7 INT; 
	SET @time_7_0=@time_0_0+(864000000000*7)
	SET @time_8_0=@time_0_0+(864000000000*8)
	SELECT @D_7=COUNT(*) FROM (SELECT DISTINCT [user_id] FROM [report_record_login] WHERE [createtime]>@time_1_0 AND [createtime]<@time_8_0) AS T
	SELECT @N_7=COUNT(*) FROM [tg_user] WHERE [createtime]>@time_7_0 AND [createtime]<@time_8_0
	SELECT @R_7=COUNT(*) FROM [tg_user] WHERE [createtime]<@time_7_0
	--30日留存
	DECLARE @time_30_0 BIGINT,@time_31_0 BIGINT,@D_30 INT,@R_30 INT,@N_30 INT; 
	SET @time_30_0=@time_0_0+(864000000000*30)
	SET @time_31_0=@time_0_0+(864000000000*31)
	SELECT @D_30=COUNT(*) FROM (SELECT DISTINCT [user_id] FROM [report_record_login] WHERE [createtime]>@time_1_0 AND [createtime]<@time_31_0) AS T
	SELECT @N_30=COUNT(*) FROM [tg_user] WHERE [createtime]>@time_30_0 AND [createtime]<@time_31_0
	SELECT @R_30=COUNT(*) FROM [tg_user] WHERE [createtime]<@time_30_0
	SELECT @D_1 AS D_1 ,@N_1 AS N_1,@R_1 AS R_1,@D_3 AS D_3,@N_3 AS N_3,@R_3 AS R_3,@D_5 AS D_5,@N_5 AS N_5,@R_5 AS R_5,@D_7 AS D_7,@N_7 AS N_7,@R_7 AS R_7 ,@D_30 AS D_30,@N_30 AS N_30,@R_30 AS R_30
END


' 
END
