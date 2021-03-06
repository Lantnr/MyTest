IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[trigger_pay_insert]'))
DROP TRIGGER [trigger_pay_insert]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_pay]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_pay]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_pay_syn]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_pay]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_pay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [sp_pay]
@sid INT,
@_begin_time BIGINT,
@_end_time BIGINT,
@M_BEGIN BIGINT,
@M_END BIGINT
AS
BEGIN
	DECLARE @pay_count INT,@pay_number INT,@pay_taday INT,@pay_total INT,@pay_month INT;
	--今日充值人数
	SELECT @pay_number=COUNT(*) FROM (SELECT DISTINCT [user_code] FROM [tgm_record_pay] WHERE  [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time) AS T
	--今日充值次数
	SELECT @pay_count=COUNT(*) FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time	
	--当日总充值
	SELECT @pay_taday=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time
	--总充值
	SELECT @pay_total=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid 	
	--当月充值
	SELECT @pay_month=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@M_BEGIN AND [createtime]<@M_END
	IF @pay_taday IS NULL
		SET @pay_taday=0;
	IF @pay_total IS NULL
		SET @pay_total=0;
	IF @pay_month IS NULL
		SET @pay_month=0;
	SELECT @pay_count AS pay_count,@pay_number AS pay_number,@pay_taday AS pay_taday,@pay_total AS pay_total,@pay_month AS pay_month	
END

' 
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_pay_syn]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_pay_syn]
@pid BIGINT,
@_begin_time BIGINT,
@_end_time BIGINT,
@M_BEGIN BIGINT,
@M_END BIGINT
AS
BEGIN
	DECLARE @sid INT, @amount INT,@createtime BIGINT,@ID BIGINT;	
	SELECT @sid =[sid], @amount = [amount],@createtime=[createtime] FROM tgm_record_pay WHERE [id]=@pid;
	DECLARE @pay_count INT,@pay_number INT,@pay_taday INT,@pay_total INT,@pay_month INT;
	--今日充值人数
	SELECT @pay_number=COUNT(*) FROM (SELECT DISTINCT [user_code] FROM [tgm_record_pay] WHERE  [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time) AS T
	--今日充值次数
	SELECT @pay_count=COUNT(*) FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time	
	--当天总充值
	SELECT @pay_taday=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@_begin_time AND [createtime]<@_end_time
	--总充值
	SELECT @pay_total=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid 
	--当月充值
	SELECT @pay_month=SUM([amount])  FROM [tgm_record_pay] WHERE [sid]=@sid AND [createtime]>=@M_BEGIN AND [createtime]<@M_END
	IF @pay_month is NULL 
		SET @pay_month=0
	SET @pay_count+=1;
	SELECT TOP 1 @ID=id FROM [tgm_record_hours] WHERE sid=@sid ORDER BY createtime DESC
	UPDATE [tgm_record_hours] SET [pay_count]=@pay_count,[pay_number]=@pay_number,[pay_taday]=@pay_taday,[pay_total]=@pay_total,[pay_month]=@pay_month
	WHERE [id]=@ID
	DECLARE @ID_DAY BIGINT, @ID_SERVER BIGINT;
	SELECT TOP 1 @ID_DAY=id FROM [tgm_record_day] WHERE sid=@sid ORDER BY createtime DESC
	UPDATE [tgm_record_day] SET [pay_count]=@pay_count,[pay_number]=@pay_number,[pay_taday]=@pay_taday,[pay_total]=@pay_total,[pay_month]=@pay_month
	WHERE [id]=@ID_DAY
	SELECT TOP 1 @ID_SERVER=id FROM [tgm_record_server] WHERE sid=@sid ORDER BY createtime DESC
	UPDATE [tgm_record_server] SET [pay_count]=@pay_count,[pay_number]=@pay_number,[pay_taday]=@pay_taday,[pay_total]=@pay_total,[pay_month]=@pay_month
	WHERE [id]=@ID_SERVER
END


' 
END